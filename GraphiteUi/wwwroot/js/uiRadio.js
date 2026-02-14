const RADIO_GROUP_SELECTOR = '[data-slot="radio-group"]';
const RADIO_INPUT_SELECTOR = 'input[type="radio"][data-slot="radio-input"]';
const RADIO_VALUE_SELECTOR = 'input[type="hidden"][data-slot="radio-value"]';

const initializedRoots = new Set();
const radioStateByRoot = new WeakMap();

function destroyRadioGroup(root) {
    const state = root ? radioStateByRoot.get(root) : null;
    if (!state) {
        return;
    }

    root.removeEventListener('change', state.onRootChanged);
    state.form?.removeEventListener('submit', state.onFormSubmitCapture, true);

    initializedRoots.delete(root);
    radioStateByRoot.delete(root);
}

function cleanupDisconnectedRoots() {
    for (const root of Array.from(initializedRoots)) {
        if (!root.isConnected) {
            destroyRadioGroup(root);
        }
    }
}

function setupRadioGroup(root) {
    if (!root) {
        return;
    }

    const existingState = radioStateByRoot.get(root);
    if (existingState) {
        existingState.refresh();
        return;
    }

    const valueInput = root.querySelector(RADIO_VALUE_SELECTOR);
    if (!valueInput) {
        return;
    }

    const getOptions = () => Array.from(root.querySelectorAll(RADIO_INPUT_SELECTOR));

    const syncHiddenFromChecked = () => {
        const selected = root.querySelector(`${RADIO_INPUT_SELECTOR}:checked`);
        valueInput.value = selected?.value || '';
    };

    const syncCheckedFromHidden = () => {
        const options = getOptions();
        const targetValue = valueInput.value || '';

        for (const option of options) {
            option.checked = targetValue.length > 0 && option.value === targetValue;
        }
    };

    const onRootChanged = (event) => {
        const target = event.target;

        if (target instanceof HTMLInputElement && target.matches(RADIO_INPUT_SELECTOR)) {
            syncHiddenFromChecked();
        }
    };

    const form = root.closest('form');
    const onFormSubmitCapture = () => {
        syncHiddenFromChecked();
    };

    const refresh = () => {
        syncCheckedFromHidden();
        syncHiddenFromChecked();
    };

    root.addEventListener('change', onRootChanged);
    form?.addEventListener('submit', onFormSubmitCapture, true);

    radioStateByRoot.set(root, {
        form,
        onRootChanged,
        onFormSubmitCapture,
        refresh
    });

    initializedRoots.add(root);
    refresh();
}

export function refreshUiRadios(root = document) {
    cleanupDisconnectedRoots();

    if (!root) {
        return;
    }

    root.querySelectorAll(RADIO_GROUP_SELECTOR).forEach((group) => {
        if (group.isConnected) {
            setupRadioGroup(group);
        }
    });
}
