# Flyout Menu Component (D20Tek.BlazorComponents.Menus)

## Understanding
Build a new, generic, reusable self-positioning Flyout/Popover Menu package `D20Tek.BlazorComponents.Menus`. It must be app-agnostic (Fortuna is only a motivating example, no Fortuna vocabulary in the API), support a kebab or custom trigger, both data-driven entries and templated child components, collision-aware placement via a minimal isolated JS module, full keyboard a11y, efficient `--d20tek-menu-*` token theming (light-default + auto dark, color-mix-derived states), a static web asset, bUnit + geometry tests, docs, changelog, and inclusion in the `.All` meta-package.

## Assumptions
- Package mirrors Modal packaging conventions (SDK.Razor, multi-target net9.0;net10.0 via `src/Directory.Build.props`, `SupportedPlatform browser`, references Core, ships `wwwroot/Menu.css`).
- Outside-click + collision measurement use an isolated `FlyoutMenu.razor.js` module invoked with `DotNetObjectReference`, only from `OnAfterRenderAsync` (prerender-safe).
- Confirmation is left entirely to consumer callbacks.
- Entry model is a polymorphic hierarchy: `IMenuEntry` marker, `abstract record MenuInteractiveItem` base, `MenuActionItem`, `MenuLinkItem`, `MenuSeparatorItem`, `MenuHeaderItem`.
- Four templated child components map 1:1 to entry records: `MenuItem`, `MenuLink`, `MenuSeparator`, `MenuHeader`.
- New `MenuPlacement` enum (BottomStart, BottomEnd, TopStart, TopEnd, LeftStart, RightStart).
- `TriggerIcon` is a `string` css class (e.g. a Bootstrap Icons class); the built-in SVG kebab is the fallback when no class is supplied.
- `Trigger` is a `RenderFragment<FlyoutMenuContext>`; the context exposes `IsOpen` and a toggle callback so custom triggers can reflect open/active state.
- Single-open policy is group-aware: `MenuCoordinator` coordinates within a `GroupName` (default shared group), so menu bars, card kebabs, and multiple independent bars stay isolated.
- The entry hierarchy (`IMenuEntry` + records) is intentionally public and open so future additive records (e.g. submenu, shortcut/trailing content) and features (hover-open, `role="menubar"` wrapper) slot in without breaking changes. These are documented extension points, not built now.
- Theming: `--d20tek-menu-*` tokens, light defaults, `@media (prefers-color-scheme: dark)` override, hover/active/focus derived from base tokens via `color-mix` (modern browsers required, no fallbacks). Structural rules in scoped `FlyoutMenu.razor.css`; token contract in `wwwroot/Menu.css`.
- Pure `PlacementCalculator` geometry helper (flip/shift decisions) is C#-unit-testable, separate from JS DOM measurement.
- Positioning uses **portal-to-body**: on open the JS module relocates the popup element to be a direct child of `<body>` and positions it with `position: fixed` in viewport coordinates, so ancestor `overflow`/`transform`/stacking contexts cannot clip or mis-position it (satisfies R4.4/R4.8). JS detaches the node before Blazor disposes to avoid "node not found" errors; teardown removes any orphaned portal node.
- `MenuHeaderItem` renders as `role="group"` with `aria-label`; the menu references grouping semantically.
- `Size` maps to **density** tiers (item padding + `--d20tek-menu-min-tap-size`) via `MenuSizeMetadata`; width stays token-driven (`--d20tek-menu-min-width`).
- **Type-ahead** is included in v1: typing a character focuses the next matching item (standard `role="menu"` pattern).
- Open policy: do **not** open when there are no entries; **do** open when all items are disabled.
- Disposal/interop race guards: disposed checks before every `InvokeAsync`/`StateHasChanged`, interop wrapped for `JSDisconnectedException`/`ObjectDisposedException`, and `MenuCoordinator` unregisters on dispose.
- Server/SSR-safe: all JS interop gated to `OnAfterRenderAsync` (never in `OnInitialized`/`OnParametersSet`); `firstRender`-safe; the menu simply stays closed until interop is available.
- Reposition/close-on-scroll uses a capture-phase `scroll` listener on `window` (catches nested scroll containers); default is reposition-while-open (`CloseOnScroll = false`).
- Open/close is driven purely by `IsOpen` state; CSS transitions are decorative so rapid toggles cannot orphan state.
- Placement/alignment use CSS **logical properties** (`inset-inline-start`, etc.) so `start`/`end` map correctly in both LTR and RTL.

## Approach
Templated children (`MenuItem`/`MenuLink`/`MenuSeparator`/`MenuHeader`) collaborate with the container via a cascading context so both APIs produce identical markup. A static, group-aware `MenuCoordinator` enforces single-open policy. Keyboard nav (Arrow/Home/End/Enter/Space/Escape/Tab + type-ahead) and focus management live in C#; the JS module portals the popup to `<body>`, handles outside-click, throttled capture-phase resize/scroll reposition, and `getBoundingClientRect` measurement feeding `PlacementCalculator`. All interop is SSR/prerender-safe (gated to `OnAfterRenderAsync`) with disposed/`JSDisconnectedException` guards. Register in `blazor-components.slnx`, add to `.All` and the unit test project, then add tests, docs, README/CHANGELOG/ReleaseNotes updates. Keep all public naming generic and document Fortuna-style shared action lists purely as a consumer pattern/example.

## Component API (FlyoutMenu parameters)

### Trigger
| Parameter | Type | Default | Purpose |
|---|---|---|---|
| `Trigger` | `RenderFragment<FlyoutMenuContext>?` | `null` | Custom trigger content; receives a context exposing `IsOpen` and a toggle callback so triggers can show active/open state (chevrons, highlight). When null, renders built-in kebab button. |
| `TriggerIcon` | `string?` | `null` | Bootstrap Icons (or other) css class for the built-in trigger; falls back to inline SVG kebab when null. |
| `TriggerAriaLabel` | `string` | `"More actions"` | aria-label for icon-only trigger. |
| `TriggerCssClass` | `string?` | `null` | Extra classes on built-in trigger button. |

### Content
| Parameter | Type | Default | Purpose |
|---|---|---|---|
| `Items` | `IReadOnlyList<IMenuEntry>?` | `null` | Data-driven entries. |
| `ChildContent` | `RenderFragment?` | `null` | Templated `MenuItem`/`MenuLink`/... children. Rendered after `Items` when both are set. |

### Positioning
| Parameter | Type | Default | Purpose |
|---|---|---|---|
| `Placement` | `MenuPlacement` | `BottomEnd` | Preferred placement. |
| `Offset` | `int` | `6` | Gap px between trigger and menu. |
| `Boundary` | `string?` (css selector) | `null` (viewport) | Collision boundary container. |
| `FlipEnabled` | `bool` | `true` | Auto flip on overflow. |
| `ShiftEnabled` | `bool` | `true` | Auto alignment shift. |

### Interaction / dismissal
| Parameter | Type | Default | Purpose |
|---|---|---|---|
| `SingleOpen` | `bool` | `true` | Opening one closes others (scoped to `GroupName`). |
| `GroupName` | `string?` | `null` (shared default group) | Scopes the single-open policy to a named group, so independent regions (e.g. a menu bar vs. card kebabs, or two separate menu bars) coordinate independently. |
| `Id` | `string?` | `null` (auto-generated) | Stable element id for `aria-controls`/`aria-labelledby` wiring and for future wrapper coordination. |
| `CloseOnScroll` | `bool` | `false` | Close when scroll container scrolls. |
| `CloseOnOutsideClick` | `bool` | `true` | Outside click/tap dismiss. |
| `Disabled` | `bool` | `false` | Disables trigger. |

### Appearance
| Parameter | Type | Default | Purpose |
|---|---|---|---|
| `ZIndex` | `int?` | `null` | Overrides `--d20tek-menu-z-index` inline. |
| `Animate` | `bool` | `true` | Fade/scale open-close, reduced-motion aware. |
| `MenuCssClass` | `string?` | `null` | Extra classes on the popup. |
| *(inherited)* `Size` | `Size` | `Small` | From `BaseComponent`. |
| *(inherited)* `IsVisible` | `bool` | `true` | From `BaseComponent`. |

### Events / programmatic
| Member | Type | Purpose |
|---|---|---|
| `OnOpen` | `EventCallback` | Raised after open. |
| `OnClose` | `EventCallback` | Raised after close. |
| `IsOpen` | `bool` (get) + `OpenAsync()`/`CloseAsync()`/`ToggleAsync()` | Programmatic control. |

## Entry model hierarchy
```csharp
public interface IMenuEntry { }

public abstract record MenuInteractiveItem : IMenuEntry
{
	public string Label { get; init; } = string.Empty;
	public string? Icon { get; init; }                 // css class, e.g. "bi bi-pencil"
	public RenderFragment? IconContent { get; init; }  // wins over Icon when set
	public bool IsDisabled { get; init; }
	public bool IsDestructive { get; init; }
	public bool CloseOnClick { get; init; } = true;
	public string? AriaLabel { get; init; }
}

public sealed record MenuActionItem : MenuInteractiveItem
{
	public EventCallback OnClick { get; init; }
}

public sealed record MenuLinkItem : MenuInteractiveItem
{
	public string Href { get; init; } = string.Empty;
	public string? Target { get; init; }               // auto rel="noopener noreferrer" for _blank
}

public sealed record MenuSeparatorItem : IMenuEntry;

public sealed record MenuHeaderItem : IMenuEntry
{
	public string Label { get; init; } = string.Empty;
}
```

## Theming token contract (`wwwroot/Menu.css`)
- Base palette (light defaults): `--d20tek-menu-bg`, `--d20tek-menu-color`, `--d20tek-menu-border`, `--d20tek-menu-radius`, `--d20tek-menu-shadow`, `--d20tek-menu-separator-color`, `--d20tek-menu-destructive-color`.
- Layout: `--d20tek-menu-item-padding`, `--d20tek-menu-min-tap-size` (>= 40px), `--d20tek-menu-min-width`, `--d20tek-menu-z-index`, `--d20tek-menu-anim-duration`, `--d20tek-menu-offset`.
- Popup width is token-driven (`--d20tek-menu-min-width`) rather than measured from the trigger, so intrinsic-width (card kebab) and matched-width (menu bar) cases are both just token overrides.
- Derived states via `color-mix`: `--d20tek-menu-item-hover-bg`, `--d20tek-menu-item-active-bg`, `--d20tek-menu-focus-ring`.
- `@media (prefers-color-scheme: dark)` redefines only base colors; derived states recompute automatically.
- color-mix required (Chromium 111+/FF 113+/Safari 16.2+); documented, no fallbacks.

## Key Files
- src/D20Tek.BlazorComponents.Modal/* - packaging, isolated JS interop, CSS token reference pattern
- src/D20Tek.BlazorComponent.Core/BaseComponent.cs - base class, Size/attributes, builders
- src/Directory.Build.props - multi-target net9.0;net10.0 + packaging metadata
- src/D20Tek.BlazorComponents.All/* - meta-package csproj + README
- blazor-components.slnx - solution registration
- tests/D20Tek.BlazorComponents.UnitTests/* - bUnit + MSTest patterns
- README.md, ReleaseNotes.md, CHANGELOG/docs - doc conventions

## Future extension points (seams left open, not built now)
- Submenus (deferred by decision): reserved as `public sealed record MenuSubmenuItem : MenuInteractiveItem { IReadOnlyList<IMenuEntry> Items { get; init; } = []; }` plus a `MenuSubmenu` child component, opened at `RightStart` (flipping to `LeftStart` near the right edge). Purely additive: existing records and consumers are untouched, the render dispatch just gains a new `case`, and `PlacementCalculator` is reused recursively for the submenu rect. Best built alongside the `MenuBar` wrapper since nested menus are primarily a menu-bar feature and need Right/Left keyboard traversal, hover-intent open/close, and tree-aware dismissal/single-open.
- Render dispatch is a `switch` on `IMenuEntry` with a `default` that ignores unknown entry types, so a future record cannot break existing code paths.
- Menu bar: a thin `MenuBar` wrapper composing sibling `FlyoutMenu`s (`role="menubar"`, arrow Left/Right, hover-to-switch) driven via `IsOpen`/`OpenAsync`/`CloseAsync` and `GroupName`.
- Hover-open: an `OpenOnHover` parameter with the coordinator transferring the active menu to a peer.
- Item trailing content: a future `TrailingContent` on `MenuInteractiveItem` for shortcut hints and submenu chevrons.

## Risks & Open Questions
- bUnit cannot exercise real DOM measurement; positioning is validated via the pure `PlacementCalculator` plus loose-mode JS interop mocks in component tests.
- Static `MenuCoordinator` single-open state must unregister on dispose to avoid cross-test leakage.
- color-mix requires modern browsers; documented as a requirement, no fallbacks.
- Focus management + prefers-reduced-motion need care but are low risk.

## Steps
1. Create project `src/D20Tek.BlazorComponents.Menus/D20Tek.BlazorComponents.Menus.csproj` modeled on Modal, and register it in `blazor-components.slnx`.
2. Add `GlobalUsings.cs`, `_Imports.razor`, `Constants.cs`, the `MenuPlacement` enum, and the `FlyoutMenuContext` record (exposes `IsOpen` + toggle callback for custom triggers).
3. Add the entry model hierarchy: `IMenuEntry`, `MenuInteractiveItem` base, `MenuActionItem`, `MenuLinkItem`, `MenuSeparatorItem`, `MenuHeaderItem`.
4. Add the pure `PlacementCalculator` geometry helper (flip/shift logic from trigger rect + menu size + boundary).
5. Implement `FlyoutMenu.razor` + `FlyoutMenu.razor.cs` container (kebab/`RenderFragment<FlyoutMenuContext>` trigger, popup, Items rendering via a `switch` on `IMenuEntry` with a `default` that ignores unknown entries, `MenuSizeMetadata` density tiers, `Id` auto-generation for aria wiring, headers as `role="group"`, do-not-open-when-empty policy, events, group-aware single-open `MenuCoordinator`, keyboard/Escape/focus/type-ahead, programmatic Open/Close/Toggle, SSR-safe interop with disposed/`JSDisconnectedException` guards, IAsyncDisposable).
   - Add `MenuSizeMetadata` mapping `Size` to density css.
6. Implement templated child components `MenuItem`, `MenuLink`, `MenuSeparator`, `MenuHeader` collaborating via a cascading context.
7. Add `FlyoutMenu.razor.js` isolated module (portal-to-body relocation + teardown, outside-click, throttled capture-phase resize/scroll reposition, getBoundingClientRect measurement, dispose) with prerender guards and node-detach-before-Blazor-dispose handling.
8. Add scoped `FlyoutMenu.razor.css` (structure/animation using CSS logical properties for RTL, reduced-motion, state-driven transitions) and `wwwroot/Menu.css` token contract (`--d20tek-menu-*` incl. `--d20tek-menu-min-width`, light defaults + dark media query, color-mix-derived states).
9. Add project reference to the `.All` meta-package and the unit test project; update `.All` csproj description + README.
10. Add pure unit tests for `PlacementCalculator` flip/shift geometry.
11. Add bUnit tests: open/close, item click callback, disabled no-op, destructive styling, Escape closes + focus return, single-open policy (same group closes, different `GroupName` stays open), aria-* attributes + `Id` wiring, header `role="group"`, keyboard nav + type-ahead, empty-menu does-not-open vs all-disabled opens, density size tiers, multi-instance independence, custom trigger `IsOpen` context, data-driven vs templated parity, link target rel.
12. Add a token-contract test asserting the `--d20tek-menu-*` tokens exist in `Menu.css`.
13. Build solution and fix compilation/test failures.
14. Update docs: package README + `docs/` api-reference (if present), root README component/package tables, `CHANGELOG.md`/`ReleaseNotes.md` Added entries, with usage examples (kebab data-driven, custom trigger, templated children, `--d20tek-menu-*` theming).
