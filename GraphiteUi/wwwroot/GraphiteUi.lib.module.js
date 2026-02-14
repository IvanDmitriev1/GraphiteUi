import { refreshUiInputFields } from './js/uiInputField.js';
import { refreshUiPopups } from './js/uiPopup.js';
import { refreshUiRadios } from './js/uiRadio.js';
import { refreshUiSelects } from './js/uiSelect.js';

let enhancedLoadRegistered = false;

function refreshAll(root = document) {
    refreshUiInputFields(root);
    refreshUiPopups(root);
    refreshUiRadios(root);
    refreshUiSelects(root);
}

export function beforeWebStart() {
    refreshAll(document);
}

export function afterWebStarted(blazor) {
    if (enhancedLoadRegistered || !blazor || typeof blazor.addEventListener !== 'function') {
        return;
    }

    enhancedLoadRegistered = true;
    blazor.addEventListener('enhancedload', onEnhancedLoad);
}

function onEnhancedLoad() {
    refreshAll(document);
}
