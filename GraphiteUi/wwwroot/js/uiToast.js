const TOAST_ITEM_SELECTOR = '[data-slot="toast-item"]';
const TOAST_DISMISS_SELECTOR = '[data-slot="alert-dismissButton"]';
const WATCHED_ATTRIBUTES = ['data-duration', 'data-sticky'];

const hostStateByRoot = new WeakMap();

const toBoolean = (value) => value === 'true';

const parseIntOrDefault = (value, fallback = 0) => {
    const parsed = Number.parseInt(value ?? '', 10);
    return Number.isFinite(parsed) ? parsed : fallback;
};

function stopTimer(entry, preserveElapsed = false) {
    if (preserveElapsed && entry.deadlineAt > 0) {
        entry.remainingMs = Math.max(0, entry.deadlineAt - Date.now());
    }

    if (entry.timerId !== 0) {
        clearTimeout(entry.timerId);
    }

    entry.timerId = 0;
    entry.deadlineAt = 0;
}

function hideEntryOnAutoDismiss(entry) {
    if (!entry.item) {
        return;
    }

    const activeElement = document.activeElement;
    if (activeElement instanceof HTMLElement && entry.item.contains(activeElement)) {
        activeElement.blur();
    }

    entry.item.setAttribute('inert', '');
    entry.item.hidden = true;
}

function requestDismiss(entry, autoTimeout = false) {
    if (entry.requestedClose) {
        return;
    }

    entry.requestedClose = true;
    stopTimer(entry, false);

    if (autoTimeout) {
        hideEntryOnAutoDismiss(entry);
    }

    entry.dismissButton?.click();
}

function startTimer(entry) {
    if (entry.timerId !== 0 || entry.remainingMs <= 0 || entry.requestedClose || entry.paused || entry.sticky) {
        return;
    }

    entry.deadlineAt = Date.now() + entry.remainingMs;
    entry.timerId = window.setTimeout(() => {
        entry.timerId = 0;
        entry.deadlineAt = 0;
        entry.remainingMs = 0;
        requestDismiss(entry, true);
    }, entry.remainingMs);
}

function createEntry() {
    return {
        item: null,
        dismissButton: null,
        timerId: 0,
        deadlineAt: 0,
        durationMs: 0,
        remainingMs: 0,
        sticky: false,
        paused: false,
        pauseLockedUntilLeave: false,
        requestedClose: false,
        onMouseEnter: null,
        onMouseLeave: null,
        onDismissClick: null
    };
}

function bindDismissButton(entry, dismissButton) {
    if (entry.dismissButton === dismissButton) {
        return;
    }

    if (entry.dismissButton && entry.onDismissClick) {
        entry.dismissButton.removeEventListener('click', entry.onDismissClick);
    }

    entry.dismissButton = dismissButton;
    entry.dismissButton?.addEventListener('click', entry.onDismissClick);
}

function detachEntry(entry, preserveElapsed) {
    if (entry.item && entry.onMouseEnter) {
        entry.item.removeEventListener('mouseenter', entry.onMouseEnter);
    }

    if (entry.item && entry.onMouseLeave) {
        entry.item.removeEventListener('mouseleave', entry.onMouseLeave);
    }

    if (entry.dismissButton && entry.onDismissClick) {
        entry.dismissButton.removeEventListener('click', entry.onDismissClick);
    }

    stopTimer(entry, preserveElapsed);
    entry.item = null;
    entry.dismissButton = null;
}

function bindEntry(entry, item) {
    if (!entry.onMouseEnter) {
        entry.onMouseEnter = () => {
            if (entry.sticky || entry.requestedClose) {
                return;
            }

            if (entry.pauseLockedUntilLeave) {
                return;
            }

            entry.paused = true;
            stopTimer(entry, true);
        };
    }

    if (!entry.onMouseLeave) {
        entry.onMouseLeave = () => {
            if (entry.pauseLockedUntilLeave) {
                entry.pauseLockedUntilLeave = false;
                return;
            }

            if (entry.sticky || entry.requestedClose) {
                return;
            }

            entry.paused = false;
            startTimer(entry);
        };
    }

    if (!entry.onDismissClick) {
        entry.onDismissClick = () => {
            entry.requestedClose = true;
            stopTimer(entry, false);
        };
    }

    const dismissButton = item.querySelector(TOAST_DISMISS_SELECTOR);
    if (entry.item === item) {
        bindDismissButton(entry, dismissButton);
        return;
    }

    detachEntry(entry, true);
    entry.item = item;
    entry.pauseLockedUntilLeave = item.matches(':hover');
    bindDismissButton(entry, dismissButton);

    item.addEventListener('mouseenter', entry.onMouseEnter);
    item.addEventListener('mouseleave', entry.onMouseLeave);
}

function syncEntry(entry, item) {
    const durationMs = Math.max(0, parseIntOrDefault(item.getAttribute('data-duration'), 0));

    entry.sticky = toBoolean(item.getAttribute('data-sticky'));

    if (durationMs !== entry.durationMs) {
        entry.durationMs = durationMs;
        entry.remainingMs = durationMs;
        stopTimer(entry, false);
    }

    if (entry.requestedClose) {
        stopTimer(entry, false);
        return;
    }

    if (entry.sticky || entry.durationMs <= 0) {
        entry.paused = false;
        stopTimer(entry, false);
        return;
    }

    if (entry.paused) {
        return;
    }

    if (entry.remainingMs <= 0) {
        requestDismiss(entry, true);
        return;
    }

    startTimer(entry);
}

function refreshHost(root, hostState) {
    const seenIds = new Set();

    for (const item of root.querySelectorAll(TOAST_ITEM_SELECTOR)) {
        const toastId = parseIntOrDefault(item.getAttribute('data-toast-id'), 0);
        if (toastId <= 0) {
            continue;
        }

        seenIds.add(toastId);

        let entry = hostState.items.get(toastId);
        if (!entry) {
            entry = createEntry();
            hostState.items.set(toastId, entry);
        }

        bindEntry(entry, item);
        syncEntry(entry, item);
    }

    for (const [toastId, entry] of hostState.items) {
        if (seenIds.has(toastId)) {
            continue;
        }

        detachEntry(entry, false);
        hostState.items.delete(toastId);
    }
}

function setupHost(root) {
    if (!root) {
        return;
    }

    let hostState = hostStateByRoot.get(root);
    if (!hostState) {
        hostState = { items: new Map(), observer: null };
        hostStateByRoot.set(root, hostState);
    }

    if (!hostState.observer) {
        hostState.observer = new MutationObserver(() => refreshHost(root, hostState));
        hostState.observer.observe(root, {
            subtree: true,
            childList: true,
            attributes: true,
            attributeFilter: WATCHED_ATTRIBUTES
        });
    }

    refreshHost(root, hostState);
}

function destroyHost(root) {
    const hostState = root ? hostStateByRoot.get(root) : null;
    if (!hostState) {
        return;
    }

    hostState.observer?.disconnect();
    hostState.observer = null;

    for (const entry of hostState.items.values()) {
        detachEntry(entry, false);
    }

    hostState.items.clear();
    hostStateByRoot.delete(root);
}

export function setupUiToastHostBlazor(root) {
    setupHost(root);
}

export function destroyUiToastHostBlazor(root) {
    destroyHost(root);
}
