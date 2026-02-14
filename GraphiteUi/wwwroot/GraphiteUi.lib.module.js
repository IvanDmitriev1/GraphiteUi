import { refreshUiInputFields } from './js/uiInputField.js';
import { refreshUiPopups } from './js/uiPopup.js';
import { refreshUiRadios } from './js/uiRadio.js';
import { refreshUiSelects } from './js/uiSelect.js';

export function beforeWebStart() {
    refreshUiInputFields(document);
    refreshUiPopups(document);
    refreshUiRadios(document);
    refreshUiSelects(document);
}

export function afterWebStarted(blazor) {
    window.previousPathName = window.location.pathname;
    blazor.addEventListener('enhancedload', onEnhancedLoad);
}

function onEnhancedLoad() {
    refreshUiInputFields(document);
    refreshUiPopups(document);
    refreshUiRadios(document);
    refreshUiSelects(document);
}
