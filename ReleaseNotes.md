# Release Notes

## Release v1.11.17
* Added the **D20Tek.BlazorComponents.Tiles** package with the `Tile` and `LinkTile` components:
  * Container-agnostic tile/card layout with a media region (icon, image, or an abbreviation-avatar fallback), a title, a clamped description, and an optional footer.
  * `Tile` renders as a `<button>` and raises a `Clicked` event; `LinkTile` renders as an `<a href>` for native navigation with an optional `Target` (auto-adds `rel="noopener noreferrer"` for `_blank`), and still raises `Clicked`.
  * The whole tile is clickable except the footer, whose interactions do not bubble to the tile.
  * `Size` (default `Medium`, `None` = unstyled) maps to clamped width tiers, and `LayoutOption` (`Compact`, `Common`, `Verbose`) controls title/description line clamping.
  * The media region renders an image when `ImageUrl` is set, otherwise an icon when `IconCssClass` is set (for example, an Open Iconic `oi oi-*` class), otherwise the abbreviation avatar. When both `ImageUrl` and `IconCssClass` are set, the image takes precedence (a Debug-only console warning is emitted).
  * Abbreviation initials fall back from an explicit override, to the first letters of the first two title words, to the first letters of the first two description words, to `?`; the avatar background uses an optional `AbbreviationColor` override or an internal deterministic 16-color palette.
  * Image load failures fall back to the abbreviation avatar via a pure-Blazor `@onerror` handler (no JS interop).
  * Isolated (scoped) CSS, so no additional stylesheet link is required.
* Added the Tiles package to the **D20Tek.BlazorComponents.All** meta-package.
* Added a Tiles sample page to the D20Tek.FullSample.Wasm sample app.
* Added unit tests to cover all of the new classes and functionality.

## Release v1.11.15
* Added the **D20Tek.BlazorComponents.Pager** package with the theme-agnostic `Pager` component:
  * Fully controlled pagination via `CurrentPage`/`CurrentPageChanged`, `PageSize`/`PageSizeChanged`, and `TotalItems`.
  * Opt-in previous/next, numbered pages, first/last buttons, a "Page X of Y" description, and a page-size selector.
  * Anchored page windowing driven by two knobs, `BoundaryCount` (default 1) and `MiddleCount` (default 5, centered on the current page), with leading/trailing ellipsis.
  * Scoped CSS styled from `currentColor` (no static stylesheet to link) that adapts to any light, dark, or custom theme.
  * Responsive collapse of subcomponents via CSS container queries as the container narrows; opt out with `DisableResponsive="true"`.
  * Configurable control labels and `Size` support.
* Added the **D20Tek.BlazorComponents.Vertically** package with the `OffsetPager<T>` component:
  * Wraps `Pager` for the D20Tek.Vertically paging types, binding a `PageOf<T>` result to the pager controls.
  * Raises `OnPageQuery` with a new one-based `PagedRequest` (`PageNumber` plus `PageSize`) on navigation or page-size changes, resetting to page 1 when the page size changes.
* Added both new packages to the **D20Tek.BlazorComponents.All** meta-package.
* Added a Pager sample page to the D20Tek.FullSample.Wasm sample app.

## Release v1.11.4
* Updated package references to latest versions.
* Updated release.yml script to use Nuget Trusted Publishing.
* Added symbol package generation to release.yml script.
* Added package icon to all nuget package projects.
* Fixed up package metadata for all nuget package projects to include solution-level readme in all packages.

## Release v1.11.2
* Added app-wide default toast settings to the **D20Tek.BlazorComponents.Toast** package:
  * New `ToastDefaults` class holds presentation-only defaults - `Position`, `DefaultTimeout`, `ShowIcon`, `Dismissible`, and `Animate`.
  * `AddToast` now has an optional overload, `AddToast(Action<ToastDefaults>? configure = null)`, that lets you configure these defaults once at startup; the parameterless call still works and registers built-in defaults (non-breaking).
  * `IToastService` exposes a read-only `Defaults` property, and `ToastService.Show` seeds each toast from the configured defaults before applying any per-call `configure`.
  * `ResultToast.ShowResult<T>` (in the **D20Tek.BlazorComponents.Functionally** package) seeds its presentation options (`Position`, `SuccessTimeout` from `DefaultTimeout`, `ShowIcon`, `Dismissible`, `Animate`) from the same defaults. Result-failure toasts remain sticky by default and are not driven by `DefaultTimeout`.
  * Settings resolve in the order: component built-in defaults -> app defaults (`AddToast`) -> per-call `configure` on `Show`/`ShowResult`.
  * Added unit tests covering the DI overload, service seeding/override behavior, and ResultToast success/failure seeding.
* Changed the built-in default toast position from `BottomRight` to `BottomCenter` (applies to `ToastDefaults`, `ToastOptions`, `ToastInstance`, and `ResultToastOptions`); override it per app via `AddToast` or per call via `configure`.
* Added a `ShowResult<T>` overload that accepts a `successMessage` parameter directly, so you can set the success message without a `configure` delegate; the trailing `configure` parameter remains optional and can still override any option (including the success message).
* Added the `BusyState` class to the **D20Tek.BlazorComponent.Core** package:
  * Tracks a transient busy/submitting flag whose scope is reset via a disposable returned from `Begin()`, even when an exception is thrown.
  * Nested `Begin()` calls are reference counted, so `IsActive` only becomes `false` once every scope has been disposed.
  * Added a `Changed` event (`EventHandler`) that consumers can subscribe to in order to re-render when the busy state transitions.
  * Added `TryBegin(out IDisposable? scope)`, which starts a busy scope only when not already active, to guard against concurrent or double-submit operations.
  * Added `RunAsync(Func<CancellationToken, Task>, CancellationToken)` and `RunAsync<T>(Func<CancellationToken, Task<T>>, CancellationToken)` wrappers that run an async operation within a busy scope, removing the manual `using` boilerplate and flowing the cancellation token to the operation.
  * Added unit tests covering activation, reentrancy, change notification, the `TryBegin` guard, and the `RunAsync` wrappers.

## Release v1.11.1
* Added the `ResultView<T>` component to the **D20Tek.BlazorComponents.Functionally** package:
  * A lightweight, non-visual render-branching container that selects UI based on the state of a `Result<T>`.
  * Slot-based rendering with `Loading`, `Success` (with the value), `Failure` (with the errors), and `Empty` render fragments.
  * Adds a pending/loading state (via `IsLoading`) that neither `ResultAlert` nor `ResultValidator` covered, ideal for whole-page or data-loading scenarios.
  * `OnStateChanged` callback and `ResultViewState` enum expose the current rendered state.
  * Added unit tests covering all render states and state-transition callbacks.
  * Added a `ResultView` sample page (with buttons to toggle success/failure/loading states) to the FullSample.Wasm project.
* Promoted the shared alert variant/icon metadata to the **D20Tek.BlazorComponent.Core** package so it can be reused across components (e.g. the upcoming Toast components):
  * Added `NotificationVariant` (renamed from `AlertVariant`) and `NotificationVariantMetadata`, which maps a variant to a neutral CSS token (`info`, `success`, `warning`, `error`, `neutral`) plus a shared default icon.
  * `ResultAlert<T>` now composes its own `result-alert-{token}` CSS class from the shared token, keeping Core component-agnostic.
  * **Breaking change:** `AlertVariant` has been removed. Replace usages with `NotificationVariant` (identical members and values); the `ResultAlert<T>` `Variant`, `SuccessVariant`, and `FailureVariant` parameters now take `NotificationVariant?`.
* Added the new **D20Tek.BlazorComponents.Toast** package - a general-purpose, content-agnostic toast notification system:
  * `ToastProvider` component - a fixed-position container placed once in the layout that renders and stacks active toasts, anchored to any of nine positions (`TopLeft` ... `BottomRight`).
  * `IToastService` (registered via `AddToast()`) - an enqueue API callable from anywhere, with `Show` overloads for a `RenderFragment`, plain message, or message + `NotificationVariant`, plus `Dismiss`.
  * `ToastInstance`, `ToastOptions`, and `ToastPosition` model per-toast content, variant, position, timeout (0 = sticky), icon, dismissible, and animation settings.
  * Per-toast auto-dismiss timer, manual dismiss, max-visible cap per position, stacking, and enter animations. Ships a static CSS file (`Toast.css`) like Modal.
  * All toast CSS classes are namespaced with a `d20tek-toast` prefix (e.g. `d20tek-toast`, `d20tek-toast-success`, `d20tek-toast-animated`) so they never collide with CSS frameworks such as Bootstrap, which defines its own `.toast` component (its `.toast:not(.show){display:none}` rule would otherwise hide the notifications). If you added custom overrides against the earlier unprefixed class names, update them to the `d20tek-toast*` names.
  * Added comprehensive unit tests for the service, options, models, position metadata, and host component.
* Added the `ResultToast<T>` specialization to the **D20Tek.BlazorComponents.Functionally** package:
  * `IToastService.ShowResult<T>` extension methods that map a `Result<T>` onto a toast via composition (no component inheritance).
  * Success toasts auto-dismiss; failure toasts are sticky. Reuses `ResultAlert` ideas: `ErrorFormatter`, `GroupErrorsByCode`, and `MaxErrorsShown` through `ResultToastOptions<T>`.
  * Added unit tests covering success/failure mapping, formatters, grouping, error limits, and display options.
* Added a `Toast` sample page (generic variant/position/sticky controls plus `ResultToast` success/failure demos) to the FullSample.Wasm project, and included Toast in the `D20Tek.BlazorComponents.All` meta-package.
* **Breaking change:** Renamed the **D20Tek.BlazorComponents.ResultValidator** package to **D20Tek.BlazorComponents.Functionally**:
  * The new name reflects the package's broader scope - it hosts a growing set of Blazor UI components that map `D20Tek.Functional` concepts (`Result`/`Error`) into the UI, including `ResultValidator`, `ResultAlert`, `ResultView`, and `ResultToast`.
  * **Action required:** replace the `D20Tek.BlazorComponents.ResultValidator` `PackageReference` with `D20Tek.BlazorComponents.Functionally`. No code changes are needed - the root namespace remains `D20Tek.BlazorComponents`, and all component types (including the `ResultValidator` component), DI extensions (`AddResultValidator`), and options classes keep their names.
  * Change was done now, shortly after the package's initial release, to minimize disruption before wider adoption.

## Release v1.10.18
* Introduced the **D20Tek.BlazorComponents.ResultValidator** package for surfacing Result/Error outcomes in Blazor forms and UI:
  * `ResultValidator` component - Integrates with `EditContext` to map operation `Error`s into Blazor form validation messages via a `ValidationMessageStore`.
  * `HandleResult` / `HandleResultAsync` - Process a `Result<T>`, invoke success callbacks on success, and push per-field validation errors on failure.
  * Per-field errors auto-clear when the user edits the associated field, and `ClearErrors` resets validation state on demand.
  * Pluggable field selection via `IErrorFieldSelector` with built-in selectors: code-as-field, display names, JSON property names, prefix stripping, delegate-based, and composite selectors.
  * `AddResultValidator` DI extension with a fluent `ResultValidatorOptions` API (`UseCodeAsField`, `UseDisplayNames`, `UseJsonPropertyNames`, `StripPrefixes`, `Compose`, `UseSelector`).
  * `ResultAlert<T>` component - Renders success/error/empty states for a `Result<T>` with customizable content, icons, header/footer, error grouping and limits, size and alert variants, borders, elevation, animation, and a dismissible close button.
  * Added unit tests covering the validator, selectors, and alert component.
* Added the **D20Tek.BlazorComponents.All** meta-package that bundles the full component suite (Markdown, Modal, ResultValidator, Spinner, Timer, TogglePanel, ToggleSwitch) behind a single package reference for easy installation.
* Updated all component projects to the latest versions of their dependencies.

## Release v1.10.11
- Implemented codeblock copy button on HTML converted markdown.
- Component has a ShowCopyButton parameter to customize whether the button is shown.
- Added codeblocks to the MarkdownView sample page to show behavior.
- Added unit tests for new behavior.
- The copy code button copies everything in the inner codeblock to the clipboard.

## Release v1.10.10
* Implemented TogglePanel component that toggles hiding and showing its inner content.
* Inner content is supplied as ChildContent parameter.
* Added unit tests for TogglePanel.
* Added sample page for TogglePanel component to the FullSample.Wasm project.

## Release v1.10.9
* Implemented MessageBox component with service-based architecture for displaying alerts and confirmation dialogs:
  * Service-Based API - Inject IMessageBoxService to show dialogs from anywhere in the application
  * 5 Message Types - Information, Success, Warning, Error, Question with built-in SVG icons
  * 4 Button Configurations - Ok, OkCancel, YesNo, YesNoCancel with MessageBoxResult return values
  * Position Variants - Top (default), Center, Bottom of viewport
  * Async/Await Pattern - All methods return Task or Task<MessageBoxResult> for proper async flow
  * Helper Methods - ShowAsync, ConfirmAsync, ShowErrorAsync, ShowWarningAsync, ShowSuccessAsync
  * Consistent Styling - Reuses ModalDialog CSS classes with message-box specific additions
  * Sample Page - MessageBoxPage demonstrates all message types, button configurations, and positioning

## Release v1.10.8
* Implemented MarkdigRenderer (for IMarkdownRenderer) to take markdown text and render it as html.
* Implemented MarkdownView that has Markdown parameter and used IMarkdownRenderer to convert it to html.
* Implemented AddMarkdownRenderer helper to register default renderer with DI.
* Added unit tests for this component and helper classes.
* Added MarkdownViewPage to the FullSample.Wasm project to show how this component can be used.

## Release v1.10.6
* Implemented ModalFormDialog component with built-in EditForm support:
  * EditForm Integration - Wraps body and footer in EditForm for native form submission
  * Validation Support - Compatible with DataAnnotationsValidator and any Blazor validator
  * OnValidSubmit / OnInvalidSubmit - Separate callbacks for valid and invalid form submissions
  * OnCancel callback - Fires when Cancel or Close button is clicked
  * Required Model parameter - Passed directly to EditForm
  * Consistent styling - Reuses all ModalDialog CSS classes

## Release v1.10.5
* Implemented Modal component with basic modal dialog functionality:
  * Native HTML5 Dialog - Built on <dialog> element for built-in accessibility, focus trapping, and backdrop support
  * Customizable Header - Title and optional summary text
  * Flexible Content - Render any Blazor content in the modal body
  * Configurable Buttons - Show/hide Cancel and Submit buttons with custom text
  * Size Variants - Small, Medium (default), Large, ExtraLarge
  * Position Variants - Top, Center (default), Bottom of viewport
  * Event Callbacks - OnClose and OnSubmit for handling user actions
  * Keyboard Support - Native Escape key to close

## Release v1.10.1
* Updated library projects to support .net 9 & 10.
* Upgraded all dependent packages to latest versions.
* Updated build scripts to require .net 10.

## Release v1.9.4
* Updated all dependent packages to latest versions.

## Release v1.9.3
* Updated all dependent packages to latest versions.

## Release v1.9.1
* Upgrades component and sample projects to .NET 9.
* Updated all dependent packages to latest versions.

## Release v1.0.13
* Created initial ToggleSwitch project.
* Implemented basic ToggleSwitch functionality.
* Created ToggleSwitchPage sample code.
* Added unit tests for ToggleSwitch.

## Release v1.0.12
* Refactored code between TimerBase and RadialTimer to better support text-only CountdownTimer.
* Created initial Countdown timer to display countdown to a specific DateTimeOffset.
* Updated display formatting to include days, hours, min, sec.
* Added unit tests for CountdownTimer.
* Created CountdownTimer sample page to show off its functionality.
* Added splash screen for Sample app.
* Added simple LabelText for CountdownTimer.
* Added timer Size property rendering and css.

## Release v1.0.11
* Updated Timer component to show hours:min:sec.
* Added invalid error handling for Timer properties that deal with time intervals.
* Refactored timer code into base classes that could be used for new Timer components.
* Created SpanTimer component with TimerDurationSpan property, which takes a TimeSpan object as input.
* Updated sample project and unit tests for refactoring and new component.
* Refactored time display formatting into helper class.

## Release v1.0.10
* Created simple Timer component project and add Timer page to Sample app.
* Moved the shared project into its own Core package and change dependencies for consuming it.
* Created new BaseComponent with properties and abstract methods used for all Blazor components.
* Created basic Timer component that supports minutes and seconds with a radial display.
* Added TimeRemaining property and IDisposable implementation to cleanup system Timer.
* Added Size functionality to Timer component.
* Added background color parameter for elapsed section.
* Added color and time interval customization to TimerPage.
* Added custom expiration message to show once timer completes.

## Release v1.0.7
* Initial Spinner component project and unit tests.
* Created full Blazor sample to show usage of various components with first page about Spinners.
* Build actions for CI/CD, official build in main, and package releases.
* Add various types of spinners: ring, square, pulse, hourglass, and dual ring.
