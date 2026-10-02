import { getUiPopupController } from './uiPopup.js';
import { createObservedRootLifecycle } from './domLifecycle.js';

const SELECT_ROOT_SELECTOR = '[data-select-root="true"]';
const ITEM_SELECTOR = '[data-slot="select-item"]';
const POPUP_READY_EVENT = 'graphite:popup-ready';
const POPUP_OPEN_CHANGE_EVENT = 'graphite:popup-open-change';

let selectIdSequence = 0;
let dynamicItemIdSequence = 0;

const selectStateByRoot = new WeakMap();
const awaitPopupReadyByRoot = new WeakMap();

const isTrue = (value) => value === 'true';

function clearAwaitPopupReady(root) {
    if (!root) {
        return;
    }

    const awaitState = awaitPopupReadyByRoot.get(root);
    if (awaitState?.handler) {
        root.removeEventListener(POPUP_READY_EVENT, awaitState.handler);
    }

    awaitPopupReadyByRoot.delete(root);
}

function registerAwaitPopupReady(root) {
    const awaitState = awaitPopupReadyByRoot.get(root);
    if (awaitState?.awaiting) {
        return;
    }

    const onPopupReady = () => {
        clearAwaitPopupReady(root);
        selectLifecycle.refresh(root);
    };

    awaitPopupReadyByRoot.set(root, { awaiting: true, handler: onPopupReady });
    root.addEventListener(POPUP_READY_EVENT, onPopupReady);
}

function getPopupApi(root) {
    const popup = getUiPopupController(root);

    if (
        popup &&
        typeof popup.open === 'function' &&
        typeof popup.close === 'function' &&
        typeof popup.isOpen === 'function' &&
        typeof popup.canInteract === 'function'
    ) {
        return popup;
    }

    return null;
}

function queryElements(root) {
    return {
        trigger: root.querySelector('[data-slot="popup-trigger"]'),
        input: root.querySelector('[data-slot="select-input"]'),
        valueInput: root.querySelector('[data-slot="select-value"]'),
        list: root.querySelector('[data-slot="select-list"]'),
        clearButton: root.querySelector('[data-slot="select-clear"]'),
        empty: root.querySelector('[data-slot="select-empty"]')
    };
}

function ensureSelectId(root) {
    if (!root.dataset.selectId) {
        root.dataset.selectId = `ui-select-${++selectIdSequence}`;
    }

    return root.dataset.selectId;
}

function destroySelect(root) {
    if (!root) {
        return;
    }

    clearAwaitPopupReady(root);

    const state = selectStateByRoot.get(root);
    if (!state) {
        return;
    }

    state.closePopup(false);
    state.setActive(null);

    state.trigger.removeEventListener('click', state.handleTriggerClick);
    state.input.removeEventListener('focus', state.handleInputFocus);
    state.input.removeEventListener('input', state.handleInput);
    state.input.removeEventListener('keydown', state.handleInputKeyDown);
    state.list.removeEventListener('click', state.handleListClick);
    state.clearButton.removeEventListener('click', state.handleClearClick);
    root.removeEventListener(POPUP_OPEN_CHANGE_EVENT, state.handlePopupOpenChange);

    selectStateByRoot.delete(root);
}

function setupSelect(root) {
    if (!root) {
        return;
    }

    const existingState = selectStateByRoot.get(root);
    if (existingState) {
        existingState.refresh();
        return;
    }

    const { trigger, input, valueInput, list, clearButton, empty } = queryElements(root);

    if (!trigger || !input || !valueInput || !list || !clearButton || !empty) {
        return;
    }

    const popup = getPopupApi(root);
    if (!popup) {
        registerAwaitPopupReady(root);
        return;
    }

    clearAwaitPopupReady(root);

    const selectId = ensureSelectId(root);
    let querying = false;
    let suppressFocusOpen = false;

    const getItems = () => Array.from(root.querySelectorAll(ITEM_SELECTOR));
    const getOpen = () => popup.isOpen();
    const canInteract = () => popup.canInteract();

    const isItemDisabled = (item) => isTrue(item.getAttribute('data-disabled')) || item.disabled;
    const getActiveItem = () => getItems().find((item) => isTrue(item.getAttribute('data-active')));
    const getSelectedItem = () => getItems().find((item) => isTrue(item.getAttribute('data-selected')));
    const getVisibleEnabledItems = () => getItems().filter((item) => !item.hidden && !isItemDisabled(item));

    const ensureItemIds = () => {
        const items = getItems();

        for (let index = 0; index < items.length; index++) {
            const item = items[index];
            if (!item.id) {
                item.id = `${selectId}-item-${index + 1}`;
            }
        }
    };

    const setActive = (item) => {
        for (const option of getItems()) {
            option.setAttribute('data-active', option === item ? 'true' : 'false');
        }

        if (!item) {
            input.removeAttribute('aria-activedescendant');
            return;
        }

        if (!item.id) {
            item.id = `${selectId}-item-dynamic-${++dynamicItemIdSequence}`;
        }

        input.setAttribute('aria-activedescendant', item.id);
        item.scrollIntoView({ block: 'nearest' });
    };

    const activateDefaultItem = (preferLast = false) => {
        const visible = getVisibleEnabledItems();

        if (visible.length === 0) {
            setActive(null);
            return;
        }

        const selected = visible.find((item) => isTrue(item.getAttribute('data-selected')));
        if (selected) {
            setActive(selected);
            return;
        }

        setActive(preferLast ? visible[visible.length - 1] : visible[0]);
    };

    const moveActive = (step) => {
        const visible = getVisibleEnabledItems();

        if (visible.length === 0) {
            setActive(null);
            return;
        }

        const current = getActiveItem();
        const currentIndex = current ? visible.indexOf(current) : -1;

        if (currentIndex < 0) {
            setActive(step < 0 ? visible[visible.length - 1] : visible[0]);
            return;
        }

        const nextIndex = (currentIndex + step + visible.length) % visible.length;
        setActive(visible[nextIndex]);
    };

    const openPopup = () => popup.open();
    const closePopup = (animate = true) => popup.close(animate);

    const updateClearState = () => {
        const hasValue = (valueInput.value || '').trim().length > 0;
        const disabled = !canInteract() || !hasValue;

        clearButton.disabled = disabled;
        clearButton.setAttribute('aria-disabled', disabled ? 'true' : 'false');
    };

    const setSelected = (item, deferClearState = false) => {
        for (const option of getItems()) {
            const selected = option === item;
            option.setAttribute('data-selected', selected ? 'true' : 'false');
            option.setAttribute('aria-selected', selected ? 'true' : 'false');
        }

        if (item) {
            valueInput.value = item.getAttribute('data-value') || '';
            input.value = item.getAttribute('data-text') || '';
        } else {
            valueInput.value = '';
        }

        // Blazor delegates clicks at document level and ignores disabled buttons.
        // Keep the clear button enabled until its current click has bubbled.
        if (deferClearState) {
            setTimeout(updateClearState, 0);
        } else {
            updateClearState();
        }
    };

    const filterItems = () => {
        const query = querying ? (input.value || '').trim().toLocaleLowerCase() : '';
        let visibleCount = 0;

        for (const item of getItems()) {
            const text = (item.getAttribute('data-text') || item.textContent || '').trim().toLocaleLowerCase();
            const match = query.length === 0 || text.includes(query);

            item.hidden = !match;

            if (match) {
                visibleCount++;
            }
        }

        empty.hidden = visibleCount > 0;

        const active = getActiveItem();
        if (active && (active.hidden || isItemDisabled(active))) {
            setActive(null);
        }
    };

    const openAndPrepare = (preferLast = false) => {
        openPopup();
        filterItems();
        activateDefaultItem(preferLast);
    };

    const selectItem = (item) => {
        if (!item || item.hidden || isItemDisabled(item) || !canInteract()) {
            return;
        }

        setSelected(item);
        closePopup();
    };

    const syncInitialSelection = () => {
        ensureItemIds();

        const selectedByAttribute = getSelectedItem();
        if (selectedByAttribute) {
            setSelected(selectedByAttribute);
            return;
        }

        const currentValue = valueInput.value || '';
        if (currentValue.length > 0) {
            const selectedByValue = getItems().find(
                (item) => (item.getAttribute('data-value') || '') === currentValue
            );

            if (selectedByValue) {
                setSelected(selectedByValue);
                return;
            }
        }

        setSelected(null);
    };

    const handleTriggerClick = () => {
        if (!canInteract()) {
            return;
        }

        openAndPrepare();
        queueMicrotask(() => input.focus());
    };

    const handleInputFocus = () => {
        if (suppressFocusOpen) {
            suppressFocusOpen = false;
            return;
        }
        if (!canInteract()) {
            return;
        }

        openAndPrepare();
    };

    const handleInput = () => {
        if (!canInteract()) {
            return;
        }

        querying = true;
        openAndPrepare();
    };

    const handleInputKeyDown = (event) => {
        if (!canInteract()) {
            return;
        }

        switch (event.key) {
            case 'Escape':
                closePopup();
                break;

            case 'ArrowDown':
                event.preventDefault();

                if (!getOpen()) {
                    openAndPrepare();
                    return;
                }

                moveActive(1);
                break;

            case 'ArrowUp':
                event.preventDefault();

                if (!getOpen()) {
                    openAndPrepare(true);
                    return;
                }

                moveActive(-1);
                break;

            case 'Enter': {
                event.preventDefault();
                if (!getOpen()) {
                    openAndPrepare();
                    return;
                }

                const target = getActiveItem() || getVisibleEnabledItems()[0];
                if (target) {
                    target.click();
                }

                break;
            }

            case 'Tab':
                closePopup(false);
                break;
        }
    };

    const handleListClick = (event) => {
        const target = event.target;
        if (!(target instanceof Element)) {
            return;
        }

        const item = target.closest(ITEM_SELECTOR);
        if (!item) {
            return;
        }

        selectItem(item);
    };

    const handleClearClick = (event) => {
        event.preventDefault();

        if (!canInteract()) {
            return;
        }

        input.value = '';
        querying = false;
        setSelected(null, true);
        filterItems();
        closePopup();

        suppressFocusOpen = true;
        queueMicrotask(() => input.focus());
    };

    const handlePopupOpenChange = (event) => {
        input.setAttribute('aria-expanded', event.detail?.open ? 'true' : 'false');
        if (event.detail?.open) {
            if (!getActiveItem()) {
                activateDefaultItem();
            }

            return;
        }

        setActive(null);
        querying = false;
        const selected = getSelectedItem();
        input.value = selected ? selected.getAttribute('data-text') || '' : '';
        filterItems();
    };

    const refresh = () => {
        querying = false;
        syncInitialSelection();
        filterItems();
        updateClearState();
        closePopup(false);
    };

    trigger.addEventListener('click', handleTriggerClick);
    input.addEventListener('focus', handleInputFocus);
    input.addEventListener('input', handleInput);
    input.addEventListener('keydown', handleInputKeyDown);
    list.addEventListener('click', handleListClick);
    clearButton.addEventListener('click', handleClearClick);
    root.addEventListener(POPUP_OPEN_CHANGE_EVENT, handlePopupOpenChange);

    selectStateByRoot.set(root, {
        trigger,
        input,
        list,
        clearButton,
        handleTriggerClick,
        handleInputFocus,
        handleInput,
        handleInputKeyDown,
        handleListClick,
        handleClearClick,
        handlePopupOpenChange,
        closePopup,
        setActive,
        refresh
    });

    refresh();
}

const selectLifecycle = createObservedRootLifecycle({
    selector: SELECT_ROOT_SELECTOR,
    setup: setupSelect,
    destroy: destroySelect,
    isInitialized: (root) => selectStateByRoot.has(root)
});

export function refreshUiSelects(root = document) {
    selectLifecycle.refresh(root);
}
