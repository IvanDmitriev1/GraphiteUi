import { createObservedRootLifecycle } from './domLifecycle.js';

const INPUT_WRAPPER_SELECTOR = '[data-slot="input-wrapper"]';
const INPUT_SELECTOR = '[data-slot="input"]';

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

function destroyInputField(wrapper) {
    const state = wrapper ? inputStateByWrapper.get(wrapper) : null;
    if (!state) {
        return;
    }

    clearTimeout(state.lateRefreshTimerId);
    state.input.removeEventListener('focus', state.refresh);
    state.input.removeEventListener('blur', state.refresh);
    state.input.removeEventListener('input', state.refresh);
    state.input.removeEventListener('change', state.refresh);

    inputStateByWrapper.delete(wrapper);
}

function setupInputField(wrapper) {
    if (!wrapper) {
        return;
    }

    const existingState = inputStateByWrapper.get(wrapper);
    if (existingState) {
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
}

const inputFieldLifecycle = createObservedRootLifecycle({
    selector: INPUT_WRAPPER_SELECTOR,
    setup: setupInputField,
    destroy: destroyInputField,
    isInitialized: (root) => inputStateByWrapper.has(root)
});

export function refreshUiInputFields(root = document) {
    inputFieldLifecycle.refresh(root);
}

export function refreshUiInputFieldsBlazor(wrapper) {
    inputFieldLifecycle.refresh(wrapper);
}

export function destroyUiInputFieldBlazor(wrapper) {
    inputFieldLifecycle.destroy(wrapper);
}
