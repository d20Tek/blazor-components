# Future Features

## Tile

A container-agnostic `Tile` component for list/grid layouts: image on top (falls back to an
abbreviation avatar), a single-line title, a two-line clamped description, an optional footer
render fragment, customizable size that shrinks on mobile, and a `Clicked` event.

### Package & Project
- New project **`D20Tek.BlazorComponents.Tile`** (`Microsoft.NET.Sdk.Razor`, multi-target `net9.0;net10.0`), root namespace `D20Tek.BlazorComponents`.
- Files: `Tile.razor`, `Tile.razor.cs`, **`Tile.razor.css` (isolated/scoped CSS)** + `GlobalUsings.cs`.
- Because CSS is isolated (Blazor auto-scopes it via generated attributes), **no class-name namespacing and no consumer `<link>` is required** (unlike Toast/Modal static CSS).
- Inherits `BaseComponent`; use `CalculateCssClasses` / `CalculateCssStyles` overrides.

### Anatomy (single root, three regions)
- `media` (top) - `<img>` when `ImageUrl` set, else abbreviation avatar (initials on colored bg).
- `body` - `title` (single line, ellipsis) + `description` (2-line `-webkit-line-clamp`).
- `footer` (optional) - renders `Footer` RenderFragment only when supplied.

### Public API
- `Title` (string), `Description` (string?), `ImageUrl` (string?), `Abbreviation` (string? override).
- `Size` (Core enum) - base dimensions.
- `Footer` (RenderFragment?), `Href` (string?, optional anchor mode).
- `Clicked` (EventCallback<MouseEventArgs>).
- Inherited: `IsVisible`, `RemainingAttributes`.

### Sizing & Responsiveness
- `TileSizeMetadata` static class maps `Size` -> modifier that sets a CSS custom property (`--tile-width`) rather than hard pixels.
- Container-agnostic: tile is a flex/inline-block item with a width; callers own the grid/flex wrapper.
- Mobile shrink via `@media` breakpoint overriding `--tile-width` (e.g. `100%`/`clamp()`); media uses `aspect-ratio`.

### Click & Accessibility
- `Href` set -> render `<a>` (native focus/keyboard/navigation); still raise `Clicked`.
- Only `Clicked` -> render `<button type="button">` (free keyboard support) or `role="button"` + `tabindex=0` + Enter/Space.
- Footer actions must `stopPropagation` so they don't trigger the tile `Clicked`.
- `aria-label` defaults to `Title`; focus-visible outline; honor `prefers-reduced-motion`.

### Abbreviation Fallback
- Compute initials: use `Abbreviation` if given, else first letters of first two words of `Title` (max 2, uppercase).
- Optional deterministic background color (hash title -> small neutral palette).
- Render `<img alt={Title}>` only when `ImageUrl` non-empty; optional `onerror` -> abbreviation later.

### CSS Notes (Tile.razor.css)
- Root: flex column, `--tile-width` custom prop, border, radius, shadow, hover elevation, `overflow: hidden`.
- Media: fixed `aspect-ratio`, `object-fit: cover`; centered flex for avatar.
- Title: `nowrap` + ellipsis. Description: 2-line `-webkit-line-clamp` box.
- Size modifiers only change custom properties (DRY).

### Testing (bUnit + MSTest, split partials)
- Rendering: image vs abbreviation fallback, title/description, footer only when supplied, size modifier class.
- Behavior: `Clicked` fires; footer click does NOT bubble; anchor mode renders `<a href>`; keyboard activation; abbreviation edge cases.
- `TileSizeMetadata` DataRow-driven mapping test.

### Rollout Checklist
- New project + `GlobalUsings.cs`; add to solution and `.All` meta-package.
- Add publish steps to `release.yml` (GitHub Packages + nuget.org), mirroring Toast.
- README: component list + package table row (note: no CSS link needed - isolated CSS).
- Sample page in `FullSample.Wasm`: responsive grid of tiles (with/without images, footer flyouts).
- `ReleaseNotes.md` entry.

### Open Questions
- Default click semantics: button-mode default, anchor-mode when `Href` set? (recommended)
- Whole-tile clickable vs explicit action area (propagation-stop handles footer).
- Abbreviation color: deterministic palette vs single neutral default.

## Toast + ResultToast&lt;T&gt; [Done]

A transient, positioned notification for showing the outcome of an operation. Unlike the
inline `ResultAlert`, a toast renders in a fixed-position container at a corner of the
viewport, auto-dismisses after a configurable timeout, and stacks multiple notifications.

This feature is split into two layers: a **general-purpose Toast** (reusable, content-agnostic)
and a thin **`ResultToast<T>`** specialization that maps a `Result<T>` onto the Toast. The
specialization uses **composition, not component inheritance** — `ResultToast` builds the
success/error content + variant and enqueues it through the generic toast service; it does not
derive from a `Toast` component.

### Layer 1: General-purpose Toast (new package `D20Tek.BlazorComponents.Toast`)

Content-agnostic toast infrastructure with no knowledge of `Result<T>`. Lives in its own
package to match the one-package-per-component philosophy and avoid mixing a general primitive
into the `ResultValidator` package.

Responsibilities/types:
- `ToastHost` component — fixed-position container placed once in the layout (e.g. in
  `MainLayout`). Subscribes to the service, renders/stacks active toasts, owns positioning
  and enter/exit animation classes.
- `IToastService` / `ToastService` (registered via an `AddToast()` DI extension) — enqueue API
  callable from anywhere; raises an event the host subscribes to. Methods to show a toast from
  a `RenderFragment` or plain message + variant, and to dismiss.
- `ToastInstance` (model) — one queued toast: unique id, content `RenderFragment`,
  `NotificationVariant`, `ToastPosition`, timeout, show-icon, dismissible, created timestamp.
- `ToastOptions` — per-call and/or global defaults: position, timeout (0 = sticky), max visible,
  show-close-button, animate.
- `ToastPosition` enum — a combined vertical + horizontal placement anchored to the host
  container: vertical (Top/Middle/Bottom) × horizontal (Left/Center/Right), yielding values like
  `TopLeft`, `TopCenter`, `TopRight`, `MiddleLeft`, ... `BottomRight` (e.g. place a toast at the
  top-left of the container). This positioning concept intentionally did not belong on the inline
  alert.
- Toast has **no sizing** concept (no `Size` parameter); width is driven by content/CSS. Sizing
  stays an alert-only concern.
- Scoped CSS for the host: corner/edge anchoring for each `ToastPosition`, stacking, and
  enter/exit animations. Note this is a static-CSS component like Modal — document the `<link>`
  requirement in the README setup section.

Behavior: auto-dismiss timer per toast (respecting timeout, 0 = manual only), manual dismiss,
max-visible cap with queueing/overflow, stacking order, and animations.

### Layer 2: ResultToast&lt;T&gt; (in `D20Tek.BlazorComponents.ResultValidator`)

Thin Result-specific layer that composes Layer 1. Depends on the new Toast package.

Responsibilities:
- Maps a `Result<T>` to toast content: success vs error variant, default success/error text,
  error formatting (reuse `ResultAlert`'s `ErrorFormatter`, `GroupErrorsByCode`, `MaxErrorsShown`
  ideas), and the matching icon.
- Preferred API: extension methods on `IToastService`, e.g.
  `ShowResult<T>(this IToastService, Result<T>, ...)` that build the content fragment + variant
  and enqueue via the service. Optionally a declarative `<ResultToast T=... Result=... />`
  component that self-enqueues on parameter change (composition — it wraps the service call,
  it does not inherit `Toast`).
- No positioning/stacking/timer logic of its own — all delegated to the Toast layer.

### Shared metadata move to Core

`ResultAlert` currently owns variant/icon metadata inside `ResultValidator`
(`AlertVariant`, `ResultAlertVariantMetadata`, `ResultAlertSizeMetadata`). Both `ResultAlert`
and the new Toast/`ResultToast` need variant + icon mapping, so promote the shared pieces to
`D20Tek.BlazorComponent.Core` to avoid a circular dependency (Toast must not depend on
ResultValidator).

- Move `AlertVariant` and the variant→CSS/icon lookup to Core, renamed to
  `NotificationVariant` and `NotificationVariantMetadata` (drops the `Alert`/`ResultAlert`
  prefix and reads well for both alerts and toasts). Apply this naming consistently.
- Keep alert-only concerns local to `ResultAlert`: `ResultAlertSizeMetadata` (and the alert
  `Size` parameter) stay in the `ResultValidator` package. Toast does not use sizing.
- Update `ResultAlert` references to the Core types (behavior unchanged); add tests to confirm
  no regressions after the move.

### Dependency direction

Core (shared variant/icon metadata) ← Toast package (generic) ← ResultValidator
(`ResultAlert` + `ResultToast<T>`). No cycles: Toast never references ResultValidator.

### Packaging / follow-ups

- Add `D20Tek.BlazorComponents.Toast` to the `.All` meta-package and to `release.yml` (both the
  GitHub Packages and public NuGet publish sections).
- Add a sample page (toast triggers + a `ResultToast` demo with success/failure buttons) and nav
  + homepage links.
- Update README (supported components, package table, static-CSS setup note) and ReleaseNotes.

### Recommended order

Implement in this sequence; each step ends in a buildable, fully tested state and respects the
dependency direction (Core ← Toast ← ResultValidator). Commit after each step.

1. **Move + rename shared types to Core** — promote the variant/icon metadata to
   `D20Tek.BlazorComponent.Core` as `NotificationVariant` / `NotificationVariantMetadata`, update
   `ResultAlert` references, keep `ResultAlertSizeMetadata` local. Build + run the existing suite
   to confirm no regressions. This is a safe refactor (no new behavior) and locks in the naming
   before more consumers exist.
2. **Create the generic `D20Tek.BlazorComponents.Toast` package** — host, service (+ `AddToast()`
   DI), `ToastInstance`, `ToastOptions`, `ToastPosition`, and scoped CSS, with its own unit tests.
   Content-agnostic; depends only on Core.
3. **Add `ResultToast<T>` specialization** in `ResultValidator` — service extension methods
   (`ShowResult<T>`) plus optional declarative component, composing the Toast layer.
4. **Wire-up / polish** — `.All` meta-package, `release.yml` (both feeds), sample page +
   nav/homepage links, README + ReleaseNotes.

Decision needed before step 1: the `AlertVariant` → `NotificationVariant` rename is a public API
breaking change for `ResultAlert` consumers. Choose either a clean rename or a rename plus an
`[Obsolete]` type alias/shim to preserve back-compat. [Decision: Rename with Obsolete shim for back-compat]

## ResultView&lt;T&gt; [Done]

A render-branching container that selects its UI based on the state of a `Result<T>`,
letting the caller supply the markup for each state. Well suited to whole-page or
data-loading scenarios rather than form submits.

Key characteristics:
- Slot-based rendering: `Loading`, `Success` (with the value), and `Failure` (with errors).
- Adds a pending/loading state that neither `ResultAlert` nor `ResultValidator` covers today.
- Lightweight/non-visual by default; styling and layout are supplied by the caller.
- Complements `ResultAlert` for cases where the entire view depends on the result state.

## OperationScope

A lightweight, opt-in cancellation primitive for Core (`D20Tek.BlazorComponents`) that owns a
`CancellationTokenSource` tied to a component's lifetime and provides ergonomic
cancel-previous-then-run semantics. It composes *alongside* `BusyState` (a component can hold
both) rather than being baked into it - cancellation and busy-tracking are orthogonal concerns,
and merging them would violate `BusyState`'s single responsibility.

### Why it is a separate primitive (not part of BusyState)
- Baking cancellation into `BusyState.RunAsync` would change behavior based on whether `default`
  was passed - a surprising, leaky API - and would give an inconsistent cancellation story across
  the library (only busy-tracked operations would be cancelable).
- Keeping it separate provides one coherent cancellation mechanism library-wide that also serves
  long-running operations that have nothing to do with the busy flag.

### Sketched public API
- `CancellationToken Token { get; }` - the read-only view to cascade to children / pass to async
  calls (the `AbortSignal` half of the owner/view split).
- `bool IsRunning { get; }` / `bool CanCancel { get; }` - for UI binding (enable/disable a Cancel
  button).
- `event EventHandler? Changed;` - so a Blazor component can re-render on state changes (mirrors
  `BusyState.Changed`).
- `void Cancel();` - the owner's cancel action (the `AbortController.abort()` half).
- `CancellationToken BeginNext(CancellationToken linkedToken = default);` - cancel the prior run
  and start a fresh one, linked to an optional outer/cascaded token.
- `Task RunAsync(Func<CancellationToken, Task> operation, CancellationToken linkedToken = default);`
  plus a `Task<T>` overload - cancel-previous + run + guaranteed cleanup (BusyState-style ergonomics).
- `void CancelAfter(TimeSpan delay);` - optional convenience (maps to `CancelAfter`/`AbortSignal.timeout`).
- `void Dispose();` - cancel-on-teardown (tie `Cancel()` to the component's `Dispose`).

### Design reminders / rationale
- **Always use a linked token source** (`CreateLinkedTokenSource`); never branch behavior on
  whether the caller passed `default`. Linked sources are cheap and always compose.
- **Owner/view split** (from `AbortController`/`AbortSignal`): the parent holds the scope and can
  `Cancel()`; children receive only the `CancellationToken` via `[CascadingParameter]`. Never
  cascade the scope itself.
- **Cancel-previous ergonomics** (from `Microsoft.VisualStudio.Threading.CancellationSeries`):
  `BeginNext` is the real ergonomic win over a hand-rolled CTS - ideal for search-as-you-type.
- **Single-flight concurrency contract**: prefer exactly one cancelable operation at a time so the
  internal `_cts` field stays unambiguous (matches the existing double-submit guard idea).
- Prior art to borrow from: `CancellationSeries` (cancel-previous) and `AbortController`/`AbortSignal`
  (owner/view split, `any` = linked tokens, `timeout` = `CancelAfter`).

### Promotion trigger (build it when any one becomes concrete)
- A sample or consumer needs **search-as-you-type / cancel-previous** semantics, or
- A sample needs a real **Submit + Cancel button pair** on client-side long-running work, or
- An actual **user request** for cancellation support arrives.

## Pager Decomposition (composable pager sub-components)

Decompose the current monolithic `Pager` into a headless coordinator plus a set of opt-in
sub-components so developers can build their own pager layout from the individual pieces
(description, page-list buttons, page-size selector, and navigation buttons), while the existing
`Pager` continues to work unchanged.

### Goals & non-goals
- **Additive and non-breaking**: the current `Pager` public API (parameters, rendered markup, CSS
  class names) stays identical; the sub-components are a new lower layer beneath it.
- **Headless coordination**: shared pagination state and windowing live in a coordinator that the
  sub-components read via a cascading value, so custom layouts stay in sync automatically.
- **Controlled model preserved**: the coordinator stays fully controlled (the consumer owns
  `CurrentPage`/`PageSize`), matching today's `Pager` and `OffsetPager<T>` behavior.
- **Non-goal - responsive collapse for custom layouts**: the container-query responsive collapse is
  supported **only** for the built-in `Pager` component. If a developer restructures `Pager` via the
  child-content escape hatch or builds their own pager from the sub-components, responsive collapse is
  explicitly not provided; those layouts are the developer's responsibility.

### Proposed pieces
- **`PagerContext`** (coordinator) - owns `CurrentPage`, `PageSize`, `TotalItems`, `BoundaryCount`,
  `MiddleCount`; computes `TotalPages` and the `PageWindow`; exposes intent methods
  (`GoToPageAsync`, `SetPageSizeAsync`) and raises `CurrentPageChanged`/`PageSizeChanged`. Provided to
  descendants as a `[CascadingParameter]`.
- **`PagerPages`** - renders the numbered page buttons plus leading/trailing ellipsis from the
  coordinator's `PageWindow` (the piece that genuinely needs the window).
- **`PagerDescription`** - renders the "Page X of Y" text from coordinator state.
- **`PagerPageSize`** - renders the page-size `<select>` bound to `PageSizeOptions`.
- **`PagerNav`** / **`PagerButton`** - first/previous/next/last navigation buttons (thin buttons that
  call coordinator intent methods and reflect disabled state).

### Rebuild the existing Pager on top of the coordinator
- Re-implement today's `Pager` internally by composing the sub-components around a `PagerContext`,
  proving the primitives are complete and keeping all existing `PagerRenderTests` /
  `PagerBehaviorTests` green.
- Add a `ChildContent` (RenderFragment) escape hatch on `Pager` so developers can reorder or omit
  regions without dropping fully to raw primitives; when `ChildContent` is supplied, `Pager` renders
  the custom layout inside its coordinator and **disables responsive collapse** for that instance.
- Align `OffsetPager<T>` to wrap the same coordinator so both packages share one composition model.

### Design reminders / rationale
- Keep the current region markup and CSS class names (`.pager__description`, `.pager__pages`,
  `.pager__button`, `.pager__page-size`, etc.) stable so extraction into sub-components is a
  mechanical refactor, not a redesign.
- `PageWindow` is already an immutable helper and can move onto the coordinator unchanged.
- Decide a small set of shared class names / CSS custom properties for the sub-components rather than
  fully isolated scoped files, so a custom layout still shares one visual language.
- New public types plus tests and docs are a meaningful API-surface commitment; ship them in a minor
  version as an additive layer.

### Promotion trigger (build it when any one becomes concrete)
- A consumer needs a **custom pager layout** (reorder/replace regions) the monolithic `Pager` cannot
  express, or
- A consumer needs to **reuse a single piece** (e.g., just the page-list buttons or the page-size
  selector) inside their own toolbar, or
- An actual **user request** for composable pager primitives arrives.
