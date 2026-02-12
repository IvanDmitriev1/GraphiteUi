const SELECT_ROOT_SELECTOR = '[data-slot="select-root"]';
const ITEM_SELECTOR = '[data-slot="select-item"]';
const CLOSE_ANIMATION_MS = 160;

let selectIdSequence = 0;

const isTrue = (value) => value === 'true';

function setupSelect(root) {
    if (!root)
        return;

    if (root.__graphiteUiSelectInit) {
        root.__graphiteUiSelectRefresh?.();
        return;
    }

    const trigger = root.querySelector('[data-slot="select-trigger"]');
    const input = root.querySelector('[data-slot="select-input"]');
    const valueInput = root.querySelector('[data-slot="select-value"]');
    const backdrop = root.querySelector('[data-slot="select-backdrop"]');
    const popup = root.querySelector('[data-slot="select-popup"]');
    const list = root.querySelector('[data-slot="select-list"]');
    const clearButton = root.querySelector('[data-slot="select-clear"]');
    const empty = root.querySelector('[data-slot="select-empty"]');

    if (!trigger || !input || !valueInput || !backdrop || !popup || !list || !clearButton || !empty)
        return;

    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    let closeTimerId = 0;

    const selectId = root.dataset.selectId || `ui-select-${++selectIdSequence}`;
    root.dataset.selectId = selectId;

    if (!popup.id)
        popup.id = `${selectId}-popup`;

    input.setAttribute('role', 'combobox');
    input.setAttribute('aria-autocomplete', 'list');
    input.setAttribute('aria-controls', popup.id);

    const getItems = () => Array.from(root.querySelectorAll(ITEM_SELECTOR));
    const getOpen = () => isTrue(trigger.getAttribute('data-open'));

    const ensureItemIds = () => {
        const items = getItems();

        for (let index = 0; index < items.length; index++) {
            const item = items[index];
            if (!item.id)
                item.id = `${selectId}-item-${index + 1}`;
        }
    };

    const isItemDisabled = (item) => isTrue(item.getAttribute('data-disabled')) || item.disabled;

    const canInteract = () => {
        const disabled = isTrue(root.getAttribute('data-disabled')) || input.disabled;
        const readOnly = isTrue(root.getAttribute('data-readonly')) || input.readOnly;
        return !disabled && !readOnly;
    };

    const getActiveItem = () => getItems().find(item => isTrue(item.getAttribute('data-active')));

    const getSelectedItem = () => getItems().find(item => isTrue(item.getAttribute('data-selected')));

    const getVisibleEnabledItems = () => getItems().filter(item => !item.hidden && !isItemDisabled(item));

    const clearCloseTimer = () => {
        if (closeTimerId === 0)
            return;

        clearTimeout(closeTimerId);
        closeTimerId = 0;
    };

    const hidePopupImmediately = () => {
        popup.hidden = true;
        backdrop.hidden = true;
    };

    const setActive = (item) => {
        for (const option of getItems()) {
            option.setAttribute('data-active', option === item ? 'true' : 'false');
        }

        if (!item) {
            input.removeAttribute('aria-activedescendant');
            return;
        }

        if (!item.id)
            item.id = `${selectId}-item-${Date.now()}`;

        input.setAttribute('aria-activedescendant', item.id);
        item.scrollIntoView({ block: 'nearest' });
    };

    const activateDefaultItem = (preferLast = false) => {
        const visible = getVisibleEnabledItems();

        if (visible.length === 0) {
            setActive(null);
            return;
        }

        const selected = visible.find(item => isTrue(item.getAttribute('data-selected')));

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

    const setOpen = (open, animate = true) => {
        const shouldOpen = open && canInteract();

        trigger.setAttribute('data-open', shouldOpen ? 'true' : 'false');
        input.setAttribute('aria-expanded', shouldOpen ? 'true' : 'false');

        if (shouldOpen) {
            clearCloseTimer();
            popup.hidden = false;
            backdrop.hidden = false;
            popup.setAttribute('data-state', 'open');
            backdrop.setAttribute('data-state', 'open');

            if (!getActiveItem())
                activateDefaultItem();

            return;
        }

        setActive(null);
        popup.setAttribute('data-state', 'closed');
        backdrop.setAttribute('data-state', 'closed');

        clearCloseTimer();

        if (!animate || prefersReducedMotion) {
            hidePopupImmediately();
            return;
        }

        closeTimerId = window.setTimeout(() => {
            hidePopupImmediately();
            closeTimerId = 0;
        }, CLOSE_ANIMATION_MS);
    };

    const updateClearState = () => {
        const hasValue = (valueInput.value || '').trim().length > 0;
        const disabled = !canInteract() || !hasValue;
        clearButton.disabled = disabled;
        clearButton.setAttribute('aria-disabled', disabled ? 'true' : 'false');
    };

    const setSelected = (item) => {
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

        updateClearState();
    };

    const filterItems = () => {
        const query = (input.value || '').trim().toLocaleLowerCase();
        let visibleCount = 0;

        for (const item of getItems()) {
            const text = (item.getAttribute('data-text') || item.textContent || '').trim().toLocaleLowerCase();
            const match = query.length === 0 || text.includes(query);
            item.hidden = !match;
            if (match)
                visibleCount++;
        }

        empty.hidden = visibleCount > 0;

        const active = getActiveItem();
        if (active && (active.hidden || isItemDisabled(active)))
            setActive(null);
    };

    const openAndPrepare = (preferLast = false) => {
        setOpen(true);
        filterItems();
        activateDefaultItem(preferLast);
    };

    const selectItem = (item) => {
        if (!item || item.hidden || isItemDisabled(item) || !canInteract())
            return;

        setSelected(item);
        setOpen(false);
    };

    const syncInitialSelection = () => {
        ensureItemIds();

        const byAttr = getSelectedItem();
        if (byAttr) {
            setSelected(byAttr);
            return;
        }

        const value = valueInput.value || '';
        if (value.length > 0) {
            const byValue = getItems().find(item => (item.getAttribute('data-value') || '') === value);
            if (byValue) {
                setSelected(byValue);
                return;
            }
        }

        setSelected(null);
    };

    const handleTriggerClick = () => {
        if (!canInteract())
            return;

        openAndPrepare();
        queueMicrotask(() => input.focus());
    };

    const handleInputFocus = () => {
        if (!canInteract())
            return;

        openAndPrepare();
    };

    const handleInput = (event) => {
        if (!canInteract())
            return;

        const currentText = event.target.value || '';
        const selected = getSelectedItem();

        if (selected && (selected.getAttribute('data-text') || '') !== currentText)
            setSelected(null);

        openAndPrepare();
    };

    const handleInputKeyDown = (event) => {
        if (!canInteract())
            return;

        switch (event.key) {
            case 'Escape':
                setOpen(false);
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
                if (!getOpen()) {
                    openAndPrepare();
                    return;
                }

                const target = getActiveItem() || getVisibleEnabledItems()[0];
                if (target) {
                    event.preventDefault();
                    target.click();
                }

                break;
            }

            case 'Tab':
                setOpen(false, false);
                break;
        }
    };

    const handleListClick = (event) => {
        const item = event.target.closest(ITEM_SELECTOR);
        if (!item)
            return;

        selectItem(item);
    };

    const handleClearClick = (event) => {
        event.preventDefault();

        if (!canInteract())
            return;

        input.value = '';
        setSelected(null);
        filterItems();
        setOpen(false);
        queueMicrotask(() => input.focus());
    };

    const handleOutsideClick = (event) => {
        if (!root.contains(event.target))
            setOpen(false);
    };

    trigger.addEventListener('click', handleTriggerClick);
    input.addEventListener('focus', handleInputFocus);
    input.addEventListener('input', handleInput);
    input.addEventListener('keydown', handleInputKeyDown);
    list.addEventListener('click', handleListClick);
    clearButton.addEventListener('click', handleClearClick);
    backdrop.addEventListener('click', () => setOpen(false));
    document.addEventListener('click', handleOutsideClick, true);

    const refresh = () => {
        syncInitialSelection();
        filterItems();
        updateClearState();
        setOpen(false, false);
    };

    root.__graphiteUiSelectRefresh = refresh;
    root.__graphiteUiSelectInit = true;

    refresh();
}

export function refreshUiSelects(root = document) {
    root.querySelectorAll(SELECT_ROOT_SELECTOR).forEach(setupSelect);
}

export function refreshUiSelectBlazor(root) {
    setupSelect(root);
}
