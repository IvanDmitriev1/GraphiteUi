const POPUP_ROOT_SELECTOR = '[data-slot="popup-root"]';
const TRIGGER_SELECTOR = '[data-slot="popup-trigger"]';
const CONTENT_SELECTOR = '[data-slot="popup-content"]';
const BACKDROP_SELECTOR = '[data-slot="popup-backdrop"]';

const CLOSE_ANIMATION_MS = 160;
const POPUP_READY_EVENT = 'graphite:popup-ready';
const POPUP_OPEN_CHANGE_EVENT = 'graphite:popup-open-change';

let documentHandlersRegistered = false;

const popupStateByRoot = new WeakMap();
const popupControllerByRoot = new WeakMap();

const isTrue = (value) => value === 'true';

function dispatchOpenChange(root, open) {
    root.dispatchEvent(new CustomEvent(POPUP_OPEN_CHANGE_EVENT, { detail: { open } }));
}

function getLivePopupStates() {
    const roots = document.querySelectorAll(POPUP_ROOT_SELECTOR);
    const states = [];

    for (const root of roots) {
        const state = popupStateByRoot.get(root);
        if (state) {
            states.push({ root, state });
        }
    }

    return states;
}

function onDocumentClick(event) {
    const target = event.target;
    if (!(target instanceof Node)) {
        return;
    }

    for (const { root, state } of getLivePopupStates()) {
        if (!state.getCloseOnOutside() || !state.getOpen()) {
            continue;
        }

        if (!root.contains(target)) {
            state.setOpen(false);
        }
    }
}

function onDocumentKeyDown(event) {
    if (event.key !== 'Escape') {
        return;
    }

    for (const { state } of getLivePopupStates()) {
        if (!state.getCloseOnEscape() || !state.getOpen()) {
            continue;
        }

        state.setOpen(false);
    }
}

function registerDocumentHandlers() {
    if (documentHandlersRegistered) {
        return;
    }

    documentHandlersRegistered = true;
    document.addEventListener('click', onDocumentClick, true);
    document.addEventListener('keydown', onDocumentKeyDown);
}

function destroyPopup(root) {
    const state = root ? popupStateByRoot.get(root) : null;
    if (!state) {
        return;
    }

    state.clearCloseTimer();
    state.trigger.removeEventListener('click', state.onTriggerClick);
    state.backdrop.removeEventListener('click', state.onBackdropClick);

    state.trigger.setAttribute('data-open', 'false');
    state.trigger.setAttribute('aria-expanded', 'false');

    state.content.hidden = true;
    state.content.setAttribute('data-state', 'closed');

    state.backdrop.hidden = true;
    state.backdrop.setAttribute('data-state', 'closed');

    popupControllerByRoot.delete(root);
    popupStateByRoot.delete(root);
}

function setupPopup(root) {
    if (!root) {
        return;
    }

    const existingState = popupStateByRoot.get(root);
    if (existingState) {
        existingState.refresh();
        return;
    }

    const trigger = root.querySelector(TRIGGER_SELECTOR);
    const content = root.querySelector(CONTENT_SELECTOR);
    const backdrop = root.querySelector(BACKDROP_SELECTOR);

    if (!trigger || !content || !backdrop) {
        return;
    }

    registerDocumentHandlers();

    let closeTimerId = 0;
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    const canInteract = () => !isTrue(root.getAttribute('data-disabled')) && !trigger.disabled;
    const getOpen = () => isTrue(trigger.getAttribute('data-open'));
    const getShowBackdrop = () => isTrue(root.getAttribute('data-show-backdrop'));
    const getCloseOnOutside = () => isTrue(root.getAttribute('data-close-outside'));
    const getCloseOnEscape = () => isTrue(root.getAttribute('data-close-escape'));
    const getTriggerMode = () => root.getAttribute('data-trigger-mode') || 'toggle';

    const clearCloseTimer = () => {
        if (closeTimerId === 0) {
            return;
        }

        clearTimeout(closeTimerId);
        closeTimerId = 0;
    };

    const hideElements = () => {
        content.hidden = true;
        backdrop.hidden = true;
    };

    const setOpen = (open, animate = true) => {
        const wasOpen = getOpen();
        const shouldOpen = open && canInteract();

        trigger.setAttribute('data-open', shouldOpen ? 'true' : 'false');
        trigger.setAttribute('aria-expanded', shouldOpen ? 'true' : 'false');

        if (shouldOpen) {
            clearCloseTimer();

            content.hidden = false;
            content.setAttribute('data-state', 'open');

            if (getShowBackdrop()) {
                backdrop.hidden = false;
                backdrop.setAttribute('data-state', 'open');
            } else {
                backdrop.hidden = true;
                backdrop.setAttribute('data-state', 'closed');
            }

            if (wasOpen !== shouldOpen) {
                dispatchOpenChange(root, true);
            }

            return;
        }

        content.setAttribute('data-state', 'closed');
        backdrop.setAttribute('data-state', 'closed');

        clearCloseTimer();

        if (!animate || prefersReducedMotion) {
            hideElements();

            if (wasOpen !== shouldOpen) {
                dispatchOpenChange(root, false);
            }

            return;
        }

        closeTimerId = window.setTimeout(() => {
            hideElements();
            closeTimerId = 0;
        }, CLOSE_ANIMATION_MS);

        if (wasOpen !== shouldOpen) {
            dispatchOpenChange(root, false);
        }
    };

    const toggleOpen = () => setOpen(!getOpen());

    const onTriggerClick = (event) => {
        if (!canInteract()) {
            return;
        }

        const mode = getTriggerMode();

        if (mode !== 'none') {
            event.preventDefault();
        }

        if (mode === 'open') {
            setOpen(true);
            return;
        }

        if (mode === 'none') {
            return;
        }

        toggleOpen();
    };

    const onBackdropClick = () => {
        setOpen(false);
    };

    const refresh = () => {
        setOpen(false, false);
    };

    trigger.addEventListener('click', onTriggerClick);
    backdrop.addEventListener('click', onBackdropClick);

    popupControllerByRoot.set(root, {
        canInteract,
        isOpen: getOpen,
        open: () => setOpen(true),
        close: (animate = true) => setOpen(false, animate),
        toggle: toggleOpen
    });

    popupStateByRoot.set(root, {
        trigger,
        content,
        backdrop,
        onTriggerClick,
        onBackdropClick,
        clearCloseTimer,
        setOpen,
        getOpen,
        getCloseOnOutside,
        getCloseOnEscape,
        refresh
    });

    refresh();
    root.dispatchEvent(new CustomEvent(POPUP_READY_EVENT));
}

export function getUiPopupController(root) {
    if (!root) {
        return null;
    }

    return popupControllerByRoot.get(root) || null;
}

export function refreshUiPopups(root = document) {
    if (!root) {
        return;
    }

    root.querySelectorAll(POPUP_ROOT_SELECTOR).forEach(setupPopup);
}

export function refreshUiPopupBlazor(root) {
    setupPopup(root);
}

export function destroyUiPopupBlazor(root) {
    destroyPopup(root);
}
