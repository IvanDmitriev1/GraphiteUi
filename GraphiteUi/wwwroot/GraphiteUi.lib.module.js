import { refreshUiInputFields } from './js/uiInputField.js';

export function beforeWebStart() {
    refreshUiInputFields(document);
}

export function afterWebStarted(blazor) {
    window.previousPathName = window.location.pathname;
    blazor.addEventListener('enhancedload', onEnhancedLoad);
}

function onEnhancedLoad() {
    refreshUiInputFields(document);
}