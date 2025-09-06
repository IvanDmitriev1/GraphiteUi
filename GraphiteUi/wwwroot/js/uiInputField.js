function setupField(wrapper) {
    if (!wrapper || wrapper.__graphiteUiInit)
        return;

    const input = wrapper.querySelector('[data-slot="input"]');
    const label = wrapper.querySelector('[data-slot="label"]');
    if (!input || !label)
        return;

    // Helpers ---------------------------------------------------------
    const setAttr = (name, on) => {
        if (on)
            wrapper.setAttribute(name, 'true');
        else
            wrapper.removeAttribute(name);
    };

    const hasInputValue = () => {
        const text = (('value' in input ? input.value : input.textContent) || input.placeholder || '').toString();
        return text.trim().length > 0;
    };

    const refresh = () => {
        const hasValue = hasInputValue();
        const focused = document.activeElement === input;
        setAttr('data-has-value', hasValue);
        setAttr('data-focused', focused);
        setAttr('data-active', focused || hasValue);
    };

    // Initial state (SSR might prerender with a value)
    refresh();

    // Wire events -----------------------------------------------------
    input.addEventListener('focus', refresh);
    input.addEventListener('blur', refresh);
    input.addEventListener('input', refresh);
    input.addEventListener('change', refresh);

    // Handle browser autofill/restore (BFCache)
    // A microtask + a later task catches late-populated values.
    queueMicrotask(refresh);
    setTimeout(refresh, 0);

    // Mark initialized to avoid double wiring
    wrapper.__graphiteUiInit = true;
}

export function initUiInputFields(root = document) {
    root.querySelectorAll('[data-slot="input-wrapper"]').forEach(setupField);
}

export function refreshUiInputFields(root = document) {
    // When Blazor patches a subtree, new wrappers appear uninitialized.
    initUiInputFields(root);
}
