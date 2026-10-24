import assert from 'node:assert/strict';
import test from 'node:test';
import { calculateUiTooltipOffset } from '../../GraphiteUi/wwwroot/js/uiTooltip.js';

test('tooltip offsets keep centered bubbles inside either viewport edge', () => {
    const width = 224;
    const viewportWidth = 390;

    assert.equal(calculateUiTooltipOffset({ left: 0, right: 24, width: 24 }, width, viewportWidth, 'Center'), -4);
    assert.equal(calculateUiTooltipOffset({ left: 376, right: 400, width: 24 }, width, viewportWidth, 'Center'), -230);
});

test('tooltip start and end alignment clamp against the viewport gutter', () => {
    const rootRect = { left: 376, right: 416, width: 40 };
    const offset = calculateUiTooltipOffset(rootRect, 224, 390, 'Start');

    assert.equal(rootRect.left + offset, 158);
    assert.equal(rootRect.right - 224 + calculateUiTooltipOffset(rootRect, 224, 390, 'End'), 158);
});

test('unknown alignment uses centered placement and in-bounds placement clears the override', () => {
    const edgeRoot = { left: 376, right: 400, width: 24 };
    const inBoundsRoot = { left: 90, right: 130, width: 40 };

    assert.equal(calculateUiTooltipOffset(edgeRoot, 224, 390, 'Unknown'), calculateUiTooltipOffset(edgeRoot, 224, 390, 'Center'));
    assert.equal(calculateUiTooltipOffset(inBoundsRoot, 100, 390, 'Center'), null);
});
