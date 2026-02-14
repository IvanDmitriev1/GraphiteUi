const INPUT_WRAPPER_SELECTOR = '[data-slot="input-wrapper"]';
const INPUT_SELECTOR = '[data-slot="input"]';

let domObserver = null;
const initializedWrappers = new Set();
const inputStateByWrapper = new WeakMap();

function setBooleanAttribute(element, name, value) {
    if (value) {
        element.setAttribute(name, 'true');
        return;
    }

    element.removeAttribute(name);
}

function hasInputValue(input) {
    if (
        input instanceof HTMLInputElement ||
        input instanceof HTMLTextAreaElement ||
        input instanceof HTMLSelectElement
    ) {
        return input.value.trim().length > 0;
    }

    return (input.textContent || '').trim().length > 0;
}

function hasPlaceholderText(input) {
    if (input instanceof HTMLInputElement || input instanceof HTMLTextAreaElement) {
        return (input.placeholder || '').trim().length > 0;
    }

    return false;
}

function collectWrappersFromNode(node) {
    if (!(node instanceof Element)) {
        return [];
    }

    const wrappers = [];

    if (node.matches(INPUT_WRAPPER_SELECTOR)) {
        wrappers.push(node);
    }

    node.querySelectorAll(INPUT_WRAPPER_SELECTOR).forEach((wrapper) => wrappers.push(wrapper));

    return wrappers;
}

function cleanupDisconnectedWrappers() {
    for (const wrapper of Array.from(initializedWrappers)) {
        if (!wrapper.isConnected) {
            destroyInputField(wrapper);
        }
    }
}

function handleDomMutations(records) {
    for (const record of records) {
        for (const addedNode of record.addedNodes) {
            for (const wrapper of collectWrappersFromNode(addedNode)) {
                setupInputField(wrapper);
            }
        }

        for (const removedNode of record.removedNodes) {
            for (const wrapper of collectWrappersFromNode(removedNode)) {
                destroyInputField(wrapper);
            }
        }
    }

    cleanupDisconnectedWrappers();
}

function ensureObserverStarted() {
    if (domObserver || !document.body) {
        return;
    }

    domObserver = new MutationObserver(handleDomMutations);
    domObserver.observe(document.body, {
        childList: true,
        subtree: true
    });
}

function destroyInputField(wrapper) {
    const state = wrapper ? inputStateByWrapper.get(wrapper) : null;
    if (!state) {
        initializedWrappers.delete(wrapper);
        return;
    }

    clearTimeout(state.lateRefreshTimerId);
    state.input.removeEventListener('focus', state.refresh);
    state.input.removeEventListener('blur', state.refresh);
    state.input.removeEventListener('input', state.refresh);
    state.input.removeEventListener('change', state.refresh);

    initializedWrappers.delete(wrapper);
    inputStateByWrapper.delete(wrapper);
}

function setupInputField(wrapper) {
    if (!wrapper) {
        return;
    }

    const existingState = inputStateByWrapper.get(wrapper);
    if (existingState) {
        initializedWrappers.add(wrapper);
        existingState.refresh();
        return;
    }

    const input = wrapper.querySelector(INPUT_SELECTOR);
    if (!input) {
        return;
    }

    const state = {
        input,
        lateRefreshTimerId: 0,
        refresh: () => {
            const focused = document.activeElement === input;
            const hasValue = hasInputValue(input);
            const hasPlaceholder = hasPlaceholderText(input);
            const isActive = focused || hasValue || hasPlaceholder;

            setBooleanAttribute(wrapper, 'data-focused', focused);
            setBooleanAttribute(wrapper, 'data-has-value', hasValue);
            setBooleanAttribute(wrapper, 'data-active', isActive);
        }
    };

    input.addEventListener('focus', state.refresh);
    input.addEventListener('blur', state.refresh);
    input.addEventListener('input', state.refresh);
    input.addEventListener('change', state.refresh);

    state.refresh();
    queueMicrotask(state.refresh);

    state.lateRefreshTimerId = window.setTimeout(() => {
        state.refresh();
        state.lateRefreshTimerId = 0;
    }, 0);

    inputStateByWrapper.set(wrapper, state);
    initializedWrappers.add(wrapper);
}

export function refreshUiInputFields(root = document) {
    if (!root) {
        return;
    }

    ensureObserverStarted();

    if (root instanceof Element && root.matches(INPUT_WRAPPER_SELECTOR)) {
        setupInputField(root);
    }

    if (typeof root.querySelectorAll === 'function') {
        root.querySelectorAll(INPUT_WRAPPER_SELECTOR).forEach(setupInputField);
    }

    cleanupDisconnectedWrappers();
}

export function refreshUiInputFieldsBlazor(wrapper) {
    setupInputField(wrapper);
}

export function destroyUiInputFieldBlazor(wrapper) {
    destroyInputField(wrapper);
}
