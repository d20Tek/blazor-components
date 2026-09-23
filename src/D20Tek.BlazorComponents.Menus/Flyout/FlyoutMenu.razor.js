// Isolated module for FlyoutMenu: portal-to-body positioning, outside-click,
// throttled reposition, roving focus, and type-ahead. All entry points are guarded
// so they no-op gracefully if elements are missing (prerender/navigation races).

const registry = new WeakMap();

const ITEM_SELECTOR = '[role="menuitem"]:not([aria-disabled="true"]):not([disabled])';

export function initialize(triggerEl, popupEl, dotNetRef, options) {
    if (!triggerEl || !popupEl) {
        return;
    }

    // Portal: relocate the popup to <body> so ancestor overflow/transform/stacking
    // cannot clip or mis-position it. Remember the original parent for teardown.
    const placeholder = document.createComment('d20tek-menu-portal');
    if (popupEl.parentNode) {
        popupEl.parentNode.insertBefore(placeholder, popupEl);
    }
    document.body.appendChild(popupEl);
    popupEl.style.position = 'fixed';

    const state = {
        triggerEl,
        popupEl,
        dotNetRef,
        options: options || {},
        placeholder,
        onDocPointerDown: null,
        onScroll: null,
        onResize: null,
        rafId: 0
    };
    registry.set(popupEl, state);

    reposition(popupEl);
    focusFirst(popupEl);

    if (state.options.closeOnOutsideClick) {
        state.onDocPointerDown = (e) => {
            if (!popupEl.contains(e.target) && !triggerEl.contains(e.target)) {
                dotNetRef.invokeMethodAsync('OnOutsideInteraction');
            }
        };
        // Defer to avoid catching the same click that opened the menu.
        setTimeout(() => document.addEventListener('pointerdown', state.onDocPointerDown, true), 0);
    }

    state.onScroll = () => {
        if (state.options.closeOnScroll) {
            dotNetRef.invokeMethodAsync('OnScrollInteraction');
        } else {
            throttledReposition(state);
        }
    };
    state.onResize = () => throttledReposition(state);

    window.addEventListener('scroll', state.onScroll, true);
    window.addEventListener('resize', state.onResize);
}

export function reposition(popupEl) {
    const state = registry.get(popupEl);
    if (!state) {
        return;
    }

    const trigger = state.triggerEl.getBoundingClientRect();
    const menu = { width: popupEl.offsetWidth, height: popupEl.offsetHeight };
    const boundary = resolveBoundary(state.options.boundary);
    const offset = state.options.offset ?? 6;

    const pos = calculate(
        trigger, menu, boundary, state.options.placement ?? 1, offset,
        state.options.flip !== false, state.options.shift !== false);

    popupEl.style.insetInlineStart = `${pos.x}px`;
    popupEl.style.insetBlockStart = `${pos.y}px`;
    popupEl.style.left = `${pos.x}px`;
    popupEl.style.top = `${pos.y}px`;
    popupEl.dataset.placement = String(pos.placement);
}

export function moveFocus(popupEl, direction) {
    const items = getItems(popupEl);
    if (items.length === 0) {
        return;
    }

    const current = document.activeElement;
    let index = items.indexOf(current);

    switch (direction) {
        case 'first': index = 0; break;
        case 'last': index = items.length - 1; break;
        case 'next': index = index < 0 ? 0 : (index + 1) % items.length; break;
        case 'previous': index = index <= 0 ? items.length - 1 : index - 1; break;
        default: return;
    }

    items[index].focus();
}

export function typeAhead(popupEl, character) {
    const items = getItems(popupEl);
    if (items.length === 0) {
        return;
    }

    const lower = character.toLowerCase();
    const current = document.activeElement;
    const start = items.indexOf(current) + 1;

    for (let i = 0; i < items.length; i++) {
        const item = items[(start + i) % items.length];
        const text = (item.textContent || '').trim().toLowerCase();
        if (text.startsWith(lower)) {
            item.focus();
            return;
        }
    }
}

export function teardown(popupEl) {
    const state = registry.get(popupEl);
    if (!state) {
        return;
    }

    if (state.onDocPointerDown) {
        document.removeEventListener('pointerdown', state.onDocPointerDown, true);
    }
    if (state.onScroll) {
        window.removeEventListener('scroll', state.onScroll, true);
    }
    if (state.onResize) {
        window.removeEventListener('resize', state.onResize);
    }
    if (state.rafId) {
        cancelAnimationFrame(state.rafId);
    }

    // Return the popup to its original location so Blazor can dispose it cleanly.
    restore(state);
    registry.delete(popupEl);
}

export function dispose(popupEl) {
    teardown(popupEl);
}

function restore(state) {
    const { popupEl, placeholder } = state;
    if (placeholder && placeholder.parentNode) {
        placeholder.parentNode.insertBefore(popupEl, placeholder);
        placeholder.remove();
    } else if (popupEl.parentNode === document.body) {
        popupEl.remove();
    }
    popupEl.style.position = '';
}

function throttledReposition(state) {
    if (state.rafId) {
        return;
    }
    state.rafId = requestAnimationFrame(() => {
        state.rafId = 0;
        reposition(state.popupEl);
    });
}

function getItems(popupEl) {
    return Array.from(popupEl.querySelectorAll(ITEM_SELECTOR));
}

function focusFirst(popupEl) {
    const items = getItems(popupEl);
    if (items.length > 0) {
        items[0].focus();
    } else {
        popupEl.focus();
    }
}

function resolveBoundary(selector) {
    if (selector) {
        const el = document.querySelector(selector);
        if (el) {
            const r = el.getBoundingClientRect();
            return { left: r.left, top: r.top, right: r.right, bottom: r.bottom };
        }
    }
    return {
        left: 0,
        top: 0,
        right: window.innerWidth,
        bottom: window.innerHeight
    };
}

// Mirrors the C# PlacementCalculator flip/shift algorithm.
// placement enum: 0 BottomStart, 1 BottomEnd, 2 TopStart, 3 TopEnd, 4 LeftStart, 5 RightStart.
function calculate(trigger, menu, boundary, placement, offset, flip, shift) {
    let side = getSide(placement);
    const alignment = getAlignment(placement);

    if (flip && !fits(side, trigger, menu, boundary, offset)) {
        const opposite = oppositeSide(side);
        if (fits(opposite, trigger, menu, boundary, offset)) {
            side = opposite;
        }
    }

    let { x, y } = coordinates(side, alignment, trigger, menu, offset);

    if (shift) {
        if (side === 'bottom' || side === 'top') {
            x = clamp(x, boundary.left, boundary.right - menu.width);
        } else {
            y = clamp(y, boundary.top, boundary.bottom - menu.height);
        }
    }

    return { x, y, placement: compose(side, alignment) };
}

function fits(side, trigger, menu, boundary, offset) {
    switch (side) {
        case 'bottom': return trigger.bottom + offset + menu.height <= boundary.bottom;
        case 'top': return trigger.top - offset - menu.height >= boundary.top;
        case 'left': return trigger.left - offset - menu.width >= boundary.left;
        case 'right': return trigger.right + offset + menu.width <= boundary.right;
        default: return true;
    }
}

function coordinates(side, alignment, trigger, menu, offset) {
    let x, y;
    switch (side) {
        case 'bottom':
            y = trigger.bottom + offset;
            x = alignment === 'start' ? trigger.left : trigger.right - menu.width;
            break;
        case 'top':
            y = trigger.top - offset - menu.height;
            x = alignment === 'start' ? trigger.left : trigger.right - menu.width;
            break;
        case 'left':
            x = trigger.left - offset - menu.width;
            y = alignment === 'start' ? trigger.top : trigger.bottom - menu.height;
            break;
        default: // right
            x = trigger.right + offset;
            y = alignment === 'start' ? trigger.top : trigger.bottom - menu.height;
            break;
    }
    return { x, y };
}

function clamp(value, min, max) {
    if (max < min) {
        return min;
    }
    return value < min ? min : (value > max ? max : value);
}

function getSide(placement) {
    if (placement === 0 || placement === 1) return 'bottom';
    if (placement === 2 || placement === 3) return 'top';
    if (placement === 4) return 'left';
    if (placement === 5) return 'right';
    return 'bottom';
}

function getAlignment(placement) {
    return (placement === 1 || placement === 3) ? 'end' : 'start';
}

function oppositeSide(side) {
    switch (side) {
        case 'bottom': return 'top';
        case 'top': return 'bottom';
        case 'left': return 'right';
        default: return 'left';
    }
}

function compose(side, alignment) {
    switch (side) {
        case 'bottom': return alignment === 'start' ? 0 : 1;
        case 'top': return alignment === 'start' ? 2 : 3;
        case 'left': return 4;
        default: return 5;
    }
}
