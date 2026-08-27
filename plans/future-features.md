# Future Features

Candidate components to extend the `Result<T>` display story beyond the current
`ResultValidator` (form validation mapping) and `ResultAlert<T>` (inline success/error banner).

## ResultToast&lt;T&gt;

A transient, positioned notification for showing the outcome of a `Result<T>` returned
from an API call. Unlike the inline `ResultAlert`, a toast renders in a fixed-position
container at a corner of the viewport, auto-dismisses after a configurable timeout, and
stacks multiple notifications.

Key characteristics:
- Fixed-position host component (e.g., `<ResultToastHost />`) placed once in the layout.
- A service (e.g., `IResultToastService`) to enqueue success/error toasts from anywhere.
- Real positioning support (Top/Bottom, Start/End) — the concept that did not belong on
  the inline alert.
- Auto-dismiss timeout, manual dismiss, stacking, and enter/exit animations.
- Reuses the variant metadata, icons, and error-formatting design from `ResultAlert`.

## ResultView&lt;T&gt;

A render-branching container that selects its UI based on the state of a `Result<T>`,
letting the caller supply the markup for each state. Well suited to whole-page or
data-loading scenarios rather than form submits.

Key characteristics:
- Slot-based rendering: `Loading`, `Success` (with the value), and `Failure` (with errors).
- Adds a pending/loading state that neither `ResultAlert` nor `ResultValidator` covers today.
- Lightweight/non-visual by default; styling and layout are supplied by the caller.
- Complements `ResultAlert` for cases where the entire view depends on the result state.
