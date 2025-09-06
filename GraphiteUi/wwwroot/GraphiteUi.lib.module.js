import { initUiInputFields, refreshUiInputFields } from './js/uiInputField.js';

export function afterWebStarted(blazor) {
    window.previousPathName = window.location.pathname;
    blazor.addEventListener('enhancedload', onEnhancedLoad);

    initUiInputFields(document);
}

function onEnhancedLoad() {
    refreshUiInputFields(document);
}
