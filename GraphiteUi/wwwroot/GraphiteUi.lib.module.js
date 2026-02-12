import { refreshUiInputFields } from './js/uiInputField.js';
import { refreshUiSelects } from './js/uiSelect.js';

export function beforeWebStart() {
    refreshUiInputFields(document);
    refreshUiSelects(document);
}

export function afterWebStarted(blazor) {
    window.previousPathName = window.location.pathname;
    blazor.addEventListener('enhancedload', onEnhancedLoad);
}

function onEnhancedLoad() {
    refreshUiInputFields(document);
    refreshUiSelects(document);
}
