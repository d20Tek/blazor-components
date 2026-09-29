# Theme Manager (D20Tek.BlazorComponents.ThemeManager)

## Understanding
Build a new, generic, reusable theming coordination package `D20Tek.BlazorComponents.ThemeManager`. Its job is to be the single source of truth for which theme is active in a Blazor app, apply the corresponding markers to the document (`data-theme`, `color-scheme`, and optional interop markers like `data-bs-theme` / `.dark`), persist the choice, react to OS `prefers-color-scheme`, and expose a C# service plus ready-made UI controls so both app code and the existing D20Tek component suite theme coherently. The work is split into two phases: **v1 MVP** delivers a robust light/dark/system manager, and **v2** adds named/custom themes and richer composition. This package controls *which* theme is active; individual components continue to self-theme via their own token contracts (`--modal-*`, `--d20tek-menu-*`, `light-dark()` defaults), keeping the manager small and the components independently usable.

## Assumptions
- Package mirrors existing library packaging conventions: `Microsoft.NET.Sdk.Razor`, multi-target `net9.0;net10.0` via `src/Directory.Build.props`, `SupportedPlatform browser`, references `D20Tek.BlazorComponent.Core`, and ships a static web asset (`wwwroot/themeManager.js` and an optional base token CSS).
- Persistence is delegated entirely to the existing `D20Tek.Blazor.BrowserStorage` package rather than a bespoke `IThemeStorage` seam. The manager depends on that package's `IBrowserStorageService` specializations (`ILocalStorageService`, `ISessionStorageService`), which already provide `GetAsync<T>`/`SetAsync<T>`/`RemoveAsync`/`ContainsKeyAsync`/`IsAvailableAsync`, a `Changed` event, and `StorageResult`/`StorageResult<T>` results. Tests use the companion `D20Tek.Blazor.BrowserStorage.Testing` package's in-memory `ILocalStorageService`/`ISessionStorageService` and `BunitContext` extensions, so no custom fake storage is needed. Cookie storage is a future feature of the browser-storage package and is deferred to ThemeManager v2 (SSR first-paint), matching its availability.
- The manager is the single source of truth that emits the markers existing components already listen for. `Menu.css` reacts to `data-theme="dark"`, `data-bs-theme="dark"`, and `.dark`; `Modal.css`/`TogglePanel` rely on `color-scheme` and `data-theme`. The manager must emit a configurable, aligned set so the whole suite themes together.
- Theme state lives in C# (`IThemeManager`, scoped service) and is projected to the DOM via an isolated JS module invoked with `IJSObjectReference` + `DotNetObjectReference`, only from `OnAfterRenderAsync` (prerender-safe).
- `ThemeMode` enum for v1: `Light`, `Dark`, `System`. When mode is `System`, the manager resolves and exposes an *effective* theme derived from `prefers-color-scheme`.
- Storage area is selectable via `ThemeManagerOptions.StorageType` (`Local` or `Session` for v1; `Cookie` added in v2 when the browser-storage package ships it); the manager resolves the matching `ILocalStorageService`/`ISessionStorageService` from DI. Storage key is configurable and must account for the browser-storage package's `KeyPrefix`.
- OS preference is observed via a `matchMedia('(prefers-color-scheme: dark)')` listener in JS that pushes changes into C# while in `System` mode; the JS listener is disposed via `IAsyncDisposable`.
- A tiny inline bootstrap snippet (documented, copy-paste into host `index.html`/`_Host`) applies the stored theme before Blazor starts to prevent FOUC. The package ships this snippet as a documented string and/or a static file.
- DI registration through `services.AddThemeManager(options => ...)` with a `ThemeManagerOptions` record controlling default mode, storage type, attribute/class/marker names, storage key, and Bootstrap-marker emission. `AddThemeManager` ensures the required browser-storage services are registered (calling `AddBrowserStorage`/`AddLocalStorage`/`AddSessionStorage` when the consumer has not already done so).
- UI components are generic and app-agnostic: `<ThemeToggle>` (two-state) and `<ThemeSelector>` (light/dark/system). Icons/labels are customizable via parameters/RenderFragments. The current sample `ThemeToggle` is the motivating example, not the API.
- All interop is SSR/prerender-safe and guarded for `JSDisconnectedException`/`ObjectDisposedException`; the service raises a change notification (`event`/`EventCallback`) so C# components re-render on theme change.
- v2 named themes are additive and non-breaking: `ThemeMode` composition and options are designed so arbitrary named themes and optional shipped token sets slot in without changing the v1 public surface.
- Package is registered in `blazor-components.slnx`, added to the `.All` meta-package and the unit test project, with bUnit + service tests, api-reference docs, README, CHANGELOG, and ReleaseNotes updates following existing conventions.

## Approach
The heart of the package is a scoped `IThemeManager`/`ThemeManager` service holding `Mode` (requested) and `EffectiveTheme` (resolved), backed directly by `D20Tek.Blazor.BrowserStorage` (`ILocalStorageService`/`ISessionStorageService`) for persistence and an isolated JS module

## Public API (v1)

### IThemeManager (scoped service)
| Member | Type | Purpose |
|---|---|---|
| `Mode` | `ThemeMode` | The requested mode (`Light`/`Dark`/`System`). |
| `EffectiveTheme` | `EffectiveTheme` (`Light`/`Dark`) | The resolved theme actually applied (resolves `System` via OS preference). |
| `IsInitialized` | `bool` | True after first interop sync completes. |
| `SetModeAsync(ThemeMode)` | `Task` | Set and persist the requested mode; recomputes effective theme and applies markers. |
| `ToggleAsync()` | `Task` | Convenience toggle between Light and Dark (documented behavior when starting from System). |
| `ThemeChanged` | `event EventHandler<ThemeChangedEventArgs>` | Raised whenever mode or effective theme changes. |

### Enums / records
| Type | Shape | Purpose |
|---|---|---|
| `ThemeMode` | `Light`, `Dark`, `System` | Requested theme mode. |
| `EffectiveTheme` | `Light`, `Dark` | Concrete applied theme. |
| `ThemeChangedEventArgs` | `Mode`, `EffectiveTheme` | Change notification payload. |
| `ThemeManagerOptions` | see below | DI-time configuration. |

### ThemeManagerOptions
| Option | Type | Default | Purpose |
|---|---|---|---|
| `DefaultMode` | `ThemeMode` | `System` | Mode when nothing is persisted. |
| `StorageType` | `ThemeStorageType` | `LocalStorage` | `LocalStorage`, `SessionStorage`, `Cookie`, `InMemory`. |
| `StorageKey` | `string` | `"d20tek-theme"` | Persistence key. |
| `ThemeAttribute` | `string` | `"data-theme"` | Attribute name written to `<html>`. |
| `ApplyColorScheme` | `bool` | `true` | Also set `color-scheme` on `<html>`. |
| `EmitBootstrapMarker` | `bool` | `false` | Also set `data-bs-theme`. |
| `DarkClassName` | `string?` | `null` | Optional class (e.g. `"dark"`) toggled on `<html>`. |

### DI extension
| Member | Purpose |
|---|---|
| `IServiceCollection AddThemeManager(this IServiceCollection, Action<ThemeManagerOptions>?)` | Registers the service, options, JS module, and (if not already present) the required `D20Tek.Blazor.BrowserStorage` services. |

### UI components
| Component | Key parameters | Purpose |
|---|---|---|
| `<ThemeToggle>` | `LightLabel`, `DarkLabel`, `LightIcon`/`DarkIcon` (`RenderFragment?`), `CssClass` | Two-state light/dark button. |
| `<ThemeSelector>` | `Display` (`Buttons`/`Dropdown`), item labels/icons | Light/Dark/System selector. |

## Phase 1 - v1 MVP (Light / Dark / System)

### Goals
A production-ready theming manager for the common case: light/dark/system, persisted, OS-aware, SSR/prerender-safe, DI-configured, with drop-in toggle/selector components and coherent markers for the whole D20Tek suite.

### Steps
1. Scaffold `src/D20Tek.BlazorComponents.ThemeManager` project (SDK.Razor, multi-target, Core reference, GlobalUsings) and register it in `blazor-components.slnx`.
2. Define `ThemeMode`, `EffectiveTheme`, and `ThemeChangedEventArgs` in the Core-aligned namespace.
3. Define `ThemeManagerOptions` and `ThemeStorageType` (`Local`, `Session`; `Cookie` reserved for v2).
4. Add the `D20Tek.Blazor.BrowserStorage` package reference and implement resolution of the selected `ILocalStorageService`/`ISessionStorageService` from `StorageType`; no bespoke storage abstraction is created.
5. Implement pure `ThemeMarkerWriter` mapping (effective theme + options) to the concrete marker set (unit-testable, no DOM I/O).
6. Author the isolated JS module `wwwroot/themeManager.js` (apply markers, read persisted state, `matchMedia` listener, disposal) exposing an `IJSObjectReference` API.
7. Implement `ThemeManager : IThemeManager, IAsyncDisposable` coordinating state, browser-storage persistence, marker writer, JS interop, and `ThemeChanged`, with disposed/`JSDisconnectedException` guards and `OnAfterRender`-gated interop init.
8. Implement `AddThemeManager` DI extension and options binding.
9. Build `<ThemeToggle>` (`.razor` + `.razor.cs` + scoped CSS) consuming the service and subscribing to `ThemeChanged`.
10. Build `<ThemeSelector>` (buttons + dropdown display modes) with customizable labels/icons.
11. Document and ship the FOUC inline bootstrap snippet (and/or static file) that applies the stored theme before Blazor starts.
12. Add the package to the `.All` meta-package and the unit test project references, including `D20Tek.Blazor.BrowserStorage.Testing` for the tests.
13. Write bUnit + service unit tests using the Testing package's in-memory storage: marker writer mapping, storage round-trips, mode/effective resolution, System + OS-change behavior (mocked interop), toggle/selector rendering and interactions, disposal guards.
14. Migrate `D20Tek.FullSample.Wasm` from bespoke `theme.js`/`ThemeToggle` to the package; remove app-specific toggle/JS; keep only genuinely app-specific chrome overrides.
15. Author api-reference docs, package README, root README setup section, CHANGELOG, and ReleaseNotes entries.
16. Run full solution build and test suite; verify green.

## Phase 2 - v2 (Custom / Named Themes)

### Goals
Grow from a light/dark switch into a true theme *manager*: arbitrary named themes, optional shipped token sets, and a cascading provider for DI-free consumption, all additive over the stable v1 API.

### Steps
1. Extend the model to support named themes: `ThemeDefinition` (name, marker value, effective base of light/dark, optional token set) and a registration API in `ThemeManagerOptions` (`AddTheme(...)`).
2. Generalize `IThemeManager` with `CurrentThemeName`, `AvailableThemes`, and `SetThemeAsync(string name)` while keeping v1 `Mode`/`ToggleAsync` working as a built-in light/dark/system triad.
3. Update `ThemeMarkerWriter` to emit named marker values (e.g. `data-theme="solarized-dark"`) plus the resolved `color-scheme` base.
4. Add an optional shipped token-set mechanism: a `wwwroot` CSS contract and helper so a named theme can carry a variable palette consumers opt into.
5. Build `<ThemeProvider>` cascading component exposing theme state to descendants without DI, plus a `<ThemeMenu>`/named-theme picker UI.
6. Extend `<ThemeSelector>` to render arbitrary registered themes (grouped by light/dark base) in addition to the built-in triad.
7. Add SSR ergonomics: documented cookie-first resolution so server/static SSR renders the correct named theme on first paint without flash.
8. Expand tests for named-theme registration, resolution, marker emission, provider cascade, and selector rendering of custom themes.
9. Update api-reference docs, README, CHANGELOG, and ReleaseNotes for the v2 surface; document extension points and migration notes.
10. Run full solution build and test suite; verify green.

## Key Files (planned)
- `src/D20Tek.BlazorComponents.ThemeManager/ThemeManager.cs` - core service and interop coordination.
- `src/D20Tek.BlazorComponents.ThemeManager/IThemeManager.cs` - public service contract.
- `src/D20Tek.BlazorComponents.ThemeManager/ThemeManagerOptions.cs` - DI configuration.
- `src/D20Tek.BlazorComponents.ThemeManager/ThemeMarkerWriter.cs` - pure marker mapping (unit-testable).
- (persistence provided by `D20Tek.Blazor.BrowserStorage`; no custom storage classes in this package).
- `src/D20Tek.BlazorComponents.ThemeManager/wwwroot/themeManager.js` - isolated interop module.
- `src/D20Tek.BlazorComponents.ThemeManager/ThemeToggle.razor(.cs/.css)` - two-state control.
- `src/D20Tek.BlazorComponents.ThemeManager/ThemeSelector.razor(.cs/.css)` - light/dark/system control.
- `src/D20Tek.BlazorComponents.ThemeManager/ServiceCollectionExtensions.cs` - `AddThemeManager`.
- `tests/D20Tek.BlazorComponents.UnitTests/ThemeManager/*` - service, storage, marker, component tests.
- `samples/D20Tek.FullSample.Wasm/*` - migrate to the package (remove bespoke `theme.js`/`ThemeToggle`).

## Risks & Open Questions
- **Marker alignment:** existing components listen for several markers (`data-theme`, `data-bs-theme`, `.dark`, `color-scheme`). The default option set must match those so the suite themes coherently; confirm the exact default marker combination to emit.
- **SSR/prerender:** localStorage is unavailable server-side. Cookie storage is the robust path for correct first paint; this depends on the `D20Tek.Blazor.BrowserStorage` cookie feature, which is a future addition there - hence cookie/SSR support is deferred to ThemeManager v2.
- **FOUC snippet distribution:** whether to ship the inline bootstrap as a documented copy-paste string, a static file, or both, and how to keep it in sync with option names.
- **FOUC key/format coupling:** the pre-render inline snippet runs before DI and must read the exact key and value encoding written by `D20Tek.Blazor.BrowserStorage` (honoring its `KeyPrefix` and `JsonSerializerDefaults.Web` encoding). Storing the theme as a simple string keeps the snippet trivial; confirm the prefix/key contract and document it.
- **`ToggleAsync` from System:** define deterministic behavior when toggling while in `System` mode (e.g. toggle relative to current effective theme, landing on an explicit Light/Dark).
- **`light-dark()` interplay:** components using `light-dark()` resolve against `color-scheme`; ensure the manager always sets `color-scheme` (unless disabled) so those components stay consistent with the marker-based ones.
- **Browser support:** `light-dark()` and `color-mix()` used by components require modern browsers; document the baseline and that the manager itself degrades gracefully (markers still apply).
- **Scope of v1 components:** confirm whether `<ThemeSelector>` dropdown mode is in v1 or deferred to v2 to keep the MVP lean.
