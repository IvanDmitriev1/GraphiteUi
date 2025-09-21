export function OpenModalDialog(dialog, dismissOnOverlayClick, closeOnEscape) {
    if (dialog.open)
        return;

    const onKeyDown = (e) => {
        if (e.key === 'Escape' && closeOnEscape === false) {
            e.stopPropagation();
            e.preventDefault();
        }
    };
    dialog.addEventListener('keydown', onKeyDown);

    dialog.addEventListener("close", (e) => {
        UnlockScroll();
        dialog.removeEventListener("keydown", onKeyDown);
    }, { once: true });

    if (dismissOnOverlayClick) {
        EnableOverlayDismiss(dialog);
    }

    dialog.showModal();
    LockScroll();
}

function LockScroll() {
    document.body.style.overflow = "hidden";
}

function UnlockScroll() {
    document.body.style.overflow = "auto";
}

function EnableOverlayDismiss(dialog, delayMs = 1000) {
    const onClick = (e) => {
        const r = dialog.getBoundingClientRect();
        const outside =
            e.clientX < r.left || e.clientX > r.right ||
            e.clientY < r.top || e.clientY > r.bottom;

        if (outside) {
            dialog.close();
        }
    };

    const timeoutId = setTimeout(() => {
        dialog.addEventListener("click", onClick);
    }, delayMs);

    dialog.addEventListener("close", () => {
        clearTimeout(timeoutId);
        dialog.removeEventListener("click", onClick);
    }, { once: true });
}