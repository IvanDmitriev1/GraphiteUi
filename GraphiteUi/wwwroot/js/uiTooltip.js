import { createObservedRootLifecycle } from './domLifecycle.js';

const TOOLTIP_ROOT_SELECTOR = '[data-slot="tooltip-root"]';
const TOOLTIP_BUBBLE_SELECTOR = '[data-slot="tooltip-bubble"]';
const VIEWPORT_GUTTER_PX = 8;

const tooltipStateByRoot = new WeakMap();
const activeTooltipRoots = new Set();
let viewportHandlersRegistered = false;

function getBubble(root) {
    return Array.from(root.querySelectorAll(TOOLTIP_BUBBLE_SELECTOR))
        .find((bubble) => bubble.parentElement === root) || null;
}

export function calculateUiTooltipOffset(rootRect, bubbleWidth, viewportWidth, alignValue) {
    const align = alignValue === 'Start' || alignValue === 'End' ? alignValue : 'Center';
    const baseLeft = align === 'Start'
        ? rootRect.left
        : align === 'End'
            ? rootRect.right - bubbleWidth
            : rootRect.left + (rootRect.width - bubbleWidth) / 2;
    const maxLeft = Math.max(VIEWPORT_GUTTER_PX, viewportWidth - VIEWPORT_GUTTER_PX - bubbleWidth);
    const clampedLeft = Math.min(Math.max(baseLeft, VIEWPORT_GUTTER_PX), maxLeft);

    if (Math.abs(clampedLeft - baseLeft) < 0.5) {
        return null;
    }

    const translateX = align === 'Center'
        ? clampedLeft - (rootRect.left + rootRect.width / 2)
        : align === 'End'
            ? clampedLeft - rootRect.right + bubbleWidth
            : clampedLeft - rootRect.left;
    return translateX;
}

function positionBubble(root, bubble) {
    const translateX = calculateUiTooltipOffset(
        root.getBoundingClientRect(),
        bubble.offsetWidth,
        document.documentElement.clientWidth,
        root.getAttribute('data-tooltip-align'));

    if (translateX === null) {
        bubble.style.removeProperty('translate');
    } else {
        bubble.style.translate = `${translateX}px 0`;
    }
}

function registerViewportHandlers() {
    if (viewportHandlersRegistered) {
        return;
    }

    viewportHandlersRegistered = true;
    const refreshPositions = () => {
        for (const root of activeTooltipRoots) {
            tooltipStateByRoot.get(root)?.refresh();
        }
    };

    window.addEventListener('resize', refreshPositions);
    window.addEventListener('scroll', refreshPositions, true);
}

function setupTooltip(root) {
    if (!root) {
        return;
    }

    const existingState = tooltipStateByRoot.get(root);
    if (existingState) {
        existingState.refresh();
        return;
    }

    registerViewportHandlers();

    let bubble = null;
    let resizeObserver;
    const refresh = () => {
        const nextBubble = getBubble(root);
        if (bubble !== nextBubble) {
            if (bubble) {
                resizeObserver.unobserve(bubble);
                bubble.style.removeProperty('translate');
            }

            bubble = nextBubble;
            if (bubble) {
                resizeObserver.observe(bubble);
            }
        }

        if (bubble) {
            positionBubble(root, bubble);
        }
    };

    resizeObserver = new ResizeObserver(refresh);
    resizeObserver.observe(root);

    const mutationObserver = new MutationObserver(refresh);
    mutationObserver.observe(root, {
        attributes: true,
        attributeFilter: ['data-tooltip-align'],
        childList: true,
        subtree: true
    });

    root.addEventListener('pointerenter', refresh);
    root.addEventListener('focusin', refresh);

    tooltipStateByRoot.set(root, { refresh, resizeObserver, mutationObserver });
    activeTooltipRoots.add(root);
    refresh();
}

function destroyTooltip(root) {
    const state = tooltipStateByRoot.get(root);
    if (!state) {
        return;
    }

    state.resizeObserver.disconnect();
    state.mutationObserver.disconnect();
    root.removeEventListener('pointerenter', state.refresh);
    root.removeEventListener('focusin', state.refresh);
    getBubble(root)?.style.removeProperty('translate');
    activeTooltipRoots.delete(root);
    tooltipStateByRoot.delete(root);
}

const tooltipLifecycle = createObservedRootLifecycle({
    selector: TOOLTIP_ROOT_SELECTOR,
    setup: setupTooltip,
    destroy: destroyTooltip,
    isInitialized: (root) => tooltipStateByRoot.has(root)
});

export function refreshUiTooltips(root = document) {
    tooltipLifecycle.refresh(root);
}

export function setupUiTooltipBlazor(root) {
    tooltipLifecycle.refresh(root);
}

export function destroyUiTooltipBlazor(root) {
    tooltipLifecycle.destroy(root);
}
