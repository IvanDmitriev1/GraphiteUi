const RADIO_GROUP_SELECTOR = '[data-slot="radio-group"]';
const RADIO_INPUT_SELECTOR = 'input[type="radio"][data-slot="radio-input"]';
const RADIO_VALUE_SELECTOR = 'input[type="hidden"][data-slot="radio-value"]';

function setupRadioGroup(root) {
    if (!root)
        return;

    if (root.__graphiteUiRadioInit) {
        root.__graphiteUiRadioRefresh?.();
        return;
    }

    const valueInput = root.querySelector(RADIO_VALUE_SELECTOR);
    if (!valueInput)
        return;

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
        if (target instanceof HTMLInputElement && target.matches(RADIO_INPUT_SELECTOR))
            syncHiddenFromChecked();
    };
    root.addEventListener('change', onRootChanged);

    const form = root.closest('form');
    const onFormSubmitCapture = () => syncHiddenFromChecked();
    form?.addEventListener('submit', onFormSubmitCapture, true);

    const refresh = () => {
        syncCheckedFromHidden();
        syncHiddenFromChecked();
    };

    root.__graphiteUiRadioRefresh = refresh;
    root.__graphiteUiRadioInit = true;

    refresh();
}

export function refreshUiRadios(root = document) {
    root.querySelectorAll(RADIO_GROUP_SELECTOR).forEach(setupRadioGroup);
}
