const POPUP_ROOT_SELECTOR = '[data-slot="popup-root"]';
const CLOSE_ANIMATION_MS = 160;
const POPUP_READY_EVENT = 'graphite:popup-ready';

const isTrue = (value) => value === 'true';

function setupPopup(root) {
    if (!root)
        return;

    if (root.__graphiteUiPopupInit) {
        root.__graphiteUiPopupRefresh?.();
        return;
    }

    const trigger = root.querySelector('[data-slot="popup-trigger"]');
    const content = root.querySelector('[data-slot="popup-content"]');
    const backdrop = root.querySelector('[data-slot="popup-backdrop"]');

    if (!trigger || !content || !backdrop)
        return;

    let closeTimerId = 0;
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    const canInteract = () => !isTrue(root.getAttribute('data-disabled')) && !trigger.disabled;
    const getOpen = () => isTrue(trigger.getAttribute('data-open'));
    const showBackdrop = () => isTrue(root.getAttribute('data-show-backdrop'));
    const closeOnOutside = () => isTrue(root.getAttribute('data-close-outside'));
    const closeOnEscape = () => isTrue(root.getAttribute('data-close-escape'));
    const triggerMode = () => root.getAttribute('data-trigger-mode') || 'toggle';

    const clearCloseTimer = () => {
        if (closeTimerId === 0)
            return;

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

            if (showBackdrop()) {
                backdrop.hidden = false;
                backdrop.setAttribute('data-state', 'open');
            }
            else {
                backdrop.hidden = true;
                backdrop.setAttribute('data-state', 'closed');
            }

            if (wasOpen !== shouldOpen) {
                root.dispatchEvent(new CustomEvent('graphite:popup-open-change', { detail: { open: true } }));
            }
            return;
        }

        content.setAttribute('data-state', 'closed');
        backdrop.setAttribute('data-state', 'closed');

        clearCloseTimer();

        if (!animate || prefersReducedMotion) {
            hideElements();
            if (wasOpen !== shouldOpen) {
                root.dispatchEvent(new CustomEvent('graphite:popup-open-change', { detail: { open: false } }));
            }
            return;
        }

        closeTimerId = window.setTimeout(() => {
            hideElements();
            closeTimerId = 0;
        }, CLOSE_ANIMATION_MS);

        if (wasOpen !== shouldOpen) {
            root.dispatchEvent(new CustomEvent('graphite:popup-open-change', { detail: { open: false } }));
        }
    };

    const toggleOpen = () => setOpen(!getOpen());

    const onTriggerClick = (event) => {
        if (!canInteract())
            return;

        const mode = triggerMode();

        if (mode !== 'none')
            event.preventDefault();

        if (mode === 'open') {
            setOpen(true);
            return;
        }

        if (mode === 'none')
            return;

        toggleOpen();
    };

    const onBackdropClick = () => setOpen(false);

    const onDocumentClick = (event) => {
        if (!closeOnOutside())
            return;

        if (!root.contains(event.target))
            setOpen(false);
    };

    const onDocumentKeyDown = (event) => {
        if (!closeOnEscape())
            return;

        if (event.key === 'Escape' && getOpen())
            setOpen(false);
    };

    trigger.addEventListener('click', onTriggerClick);
    backdrop.addEventListener('click', onBackdropClick);
    document.addEventListener('click', onDocumentClick, true);
    document.addEventListener('keydown', onDocumentKeyDown);

    const refresh = () => {
        setOpen(false, false);
    };

    root.__graphiteUiPopup = {
        canInteract,
        isOpen: getOpen,
        open: () => setOpen(true),
        close: (animate = true) => setOpen(false, animate),
        toggle: toggleOpen
    };

    root.__graphiteUiPopupRefresh = refresh;
    root.__graphiteUiPopupInit = true;

    refresh();
    root.dispatchEvent(new CustomEvent(POPUP_READY_EVENT));
}

export function refreshUiPopups(root = document) {
    root.querySelectorAll(POPUP_ROOT_SELECTOR).forEach(setupPopup);
}

export function refreshUiPopupBlazor(root) {
    setupPopup(root);
}
