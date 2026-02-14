const OVERLAY_DISMISS_DELAY_MS = 800;

let activeScrollLocks = 0;
let previousBodyOverflow = '';

const dialogStateByDialog = new WeakMap();

function lockScroll() {
    if (activeScrollLocks === 0) {
        previousBodyOverflow = document.body.style.overflow;
        document.body.style.overflow = 'hidden';
    }

    activeScrollLocks++;
}

function unlockScroll() {
    if (activeScrollLocks === 0) {
        return;
    }

    activeScrollLocks--;

    if (activeScrollLocks === 0) {
        document.body.style.overflow = previousBodyOverflow;
        previousBodyOverflow = '';
    }
}

function isOutsideDialog(dialog, event) {
    const bounds = dialog.getBoundingClientRect();

    return (
        event.clientX < bounds.left ||
        event.clientX > bounds.right ||
        event.clientY < bounds.top ||
        event.clientY > bounds.bottom
    );
}

function cleanupDialog(dialog) {
    const state = dialogStateByDialog.get(dialog);
    if (!state) {
        return;
    }

    clearTimeout(state.overlayTimerId);
    dialog.removeEventListener('keydown', state.onKeyDown);
    dialog.removeEventListener('click', state.onOverlayClick);

    dialogStateByDialog.delete(dialog);
    unlockScroll();
}

export function openModalDialog(dialog, dismissOnOverlayClick, closeOnEscape) {
    if (!dialog || dialog.open) {
        return;
    }

    cleanupDialog(dialog);

    const state = {
        overlayTimerId: 0,
        onKeyDown: (event) => {
            if (event.key === 'Escape' && closeOnEscape === false) {
                event.preventDefault();
                event.stopPropagation();
            }
        },
        onOverlayClick: (event) => {
            if (isOutsideDialog(dialog, event)) {
                dialog.close();
            }
        }
    };

    dialogStateByDialog.set(dialog, state);
    dialog.addEventListener('keydown', state.onKeyDown);
    dialog.addEventListener(
        'close',
        () => {
            cleanupDialog(dialog);
        },
        { once: true }
    );

    if (dismissOnOverlayClick) {
        state.overlayTimerId = window.setTimeout(() => {
            dialog.addEventListener('click', state.onOverlayClick);
            state.overlayTimerId = 0;
        }, OVERLAY_DISMISS_DELAY_MS);
    }

    dialog.showModal();
    lockScroll();
}

// Compatibility alias for existing C# interop calls.
export function OpenModalDialog(dialog, dismissOnOverlayClick, closeOnEscape) {
    openModalDialog(dialog, dismissOnOverlayClick, closeOnEscape);
}
