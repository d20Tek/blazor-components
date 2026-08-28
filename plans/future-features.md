# Future Features

Candidate components to extend the `Result<T>` display story beyond the current
`ResultValidator` (form validation mapping) and `ResultAlert<T>` (inline success/error banner).

## Toast + ResultToast&lt;T&gt;

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
