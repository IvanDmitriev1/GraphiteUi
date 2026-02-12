const ITEM_SELECTOR = '[data-slot="toast-item"]';
const DISMISS_SELECTOR = '[data-slot="toast-dismiss"]';
const WATCHED_ATTRIBUTES = ['data-state', 'data-duration', 'data-sticky', 'data-pause-hover'];

const toBool = (value) => value === 'true';

const toInt = (value, fallback = 0) => {
    const parsed = Number.parseInt(value ?? '', 10);
    return Number.isFinite(parsed) ? parsed : fallback;
};

function clearTimer(entry) {
    if (entry.timerId === 0) {
        return;
    }

    clearTimeout(entry.timerId);
    entry.timerId = 0;
    entry.startedAt = 0;
}

function pauseTimer(entry) {
    if (entry.timerId === 0 || entry.startedAt === 0) {
        return;
    }

    const elapsed = Date.now() - entry.startedAt;
    entry.remainingMs = Math.max(0, entry.remainingMs - elapsed);
    clearTimer(entry);
}

function requestDismiss(entry) {
    if (entry.requestedClose) {
        return;
    }

    entry.requestedClose = true;
    clearTimer(entry);
    entry.dismissButton?.click();
}

function startTimer(entry) {
    if (entry.timerId !== 0 || entry.remainingMs <= 0 || entry.requestedClose) {
        return;
    }

    entry.startedAt = Date.now();
    entry.timerId = window.setTimeout(() => {
        entry.timerId = 0;
        entry.startedAt = 0;
        entry.remainingMs = 0;
        requestDismiss(entry);
    }, entry.remainingMs);
}

function createEntry() {
    return {
        item: null,
        dismissButton: null,
        timerId: 0,
        startedAt: 0,
        durationMs: 0,
        remainingMs: 0,
        sticky: false,
        pauseOnHover: true,
        paused: false,
        requestedClose: false,
        onMouseEnter: null,
        onMouseLeave: null,
        onDismissClick: null
    };
}

function detachEntry(entry) {
    if (entry.item && entry.onMouseEnter) {
        entry.item.removeEventListener('mouseenter', entry.onMouseEnter);
    }

    if (entry.item && entry.onMouseLeave) {
        entry.item.removeEventListener('mouseleave', entry.onMouseLeave);
    }

    if (entry.dismissButton && entry.onDismissClick) {
        entry.dismissButton.removeEventListener('click', entry.onDismissClick);
    }

    clearTimer(entry);
    entry.item = null;
    entry.dismissButton = null;
}

function bindEntry(entry, item) {
    if (entry.item === item) {
        return;
    }

    detachEntry(entry);

    entry.item = item;
    entry.dismissButton = item.querySelector(DISMISS_SELECTOR);
    entry.onMouseEnter = () => {
        if (!entry.pauseOnHover || entry.sticky || entry.requestedClose) {
            return;
        }

        entry.paused = true;
        pauseTimer(entry);
    };

    entry.onMouseLeave = () => {
        if (!entry.pauseOnHover || entry.sticky || entry.requestedClose) {
            return;
        }

        entry.paused = false;
        startTimer(entry);
    };

    entry.onDismissClick = () => {
        entry.requestedClose = true;
        clearTimer(entry);
    };

    item.addEventListener('mouseenter', entry.onMouseEnter);
    item.addEventListener('mouseleave', entry.onMouseLeave);
    entry.dismissButton?.addEventListener('click', entry.onDismissClick);
}

function syncEntry(entry, item) {
    const stateName = item.getAttribute('data-state') || 'open';
    const durationMs = Math.max(0, toInt(item.getAttribute('data-duration'), 0));

    entry.pauseOnHover = toBool(item.getAttribute('data-pause-hover'));
    entry.sticky = toBool(item.getAttribute('data-sticky'));

    if (durationMs !== entry.durationMs) {
        entry.durationMs = durationMs;
        entry.remainingMs = durationMs;
        clearTimer(entry);
    }

    if (stateName === 'closing') {
        entry.requestedClose = true;
        entry.paused = false;
        clearTimer(entry);
        return;
    }

    if (entry.requestedClose) {
        clearTimer(entry);
        return;
    }

    if (entry.sticky || entry.durationMs <= 0) {
        entry.paused = false;
        clearTimer(entry);
        return;
    }

    if (entry.paused) {
        return;
    }

    if (entry.remainingMs <= 0) {
        entry.remainingMs = entry.durationMs;
    }

    startTimer(entry);
}

function refreshHost(root, hostState) {
    const seenIds = new Set();

    for (const item of root.querySelectorAll(ITEM_SELECTOR)) {
        const toastId = toInt(item.getAttribute('data-toast-id'), 0);
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

        detachEntry(entry);
        hostState.items.delete(toastId);
    }
}

function setupHost(root) {
    if (!root) {
        return;
    }

    let hostState = root.__graphiteUiToast;
    if (!hostState) {
        hostState = { items: new Map(), observer: null };
        root.__graphiteUiToast = hostState;
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
    const hostState = root?.__graphiteUiToast;
    if (!hostState) {
        return;
    }

    hostState.observer?.disconnect();
    hostState.observer = null;

    for (const entry of hostState.items.values()) {
        detachEntry(entry);
    }

    hostState.items.clear();
    delete root.__graphiteUiToast;
}

export function setupUiToastHostBlazor(root) {
    setupHost(root);
}

export function destroyUiToastHostBlazor(root) {
    destroyHost(root);
}
