# D20Tek.BlazorComponents.Pager

A content-agnostic Blazor pagination component. The `Pager` renders first/previous/numbered/next/last
controls, an optional "page X of Y" description, and an optional page-size selector.

## Features

- First, previous, numbered pages, next, and last controls (numbered + prev/next shown by default;
  first/last, description, and page-size selector are opt-in).
- MudBlazor-style windowed page list with anchored ends and ellipsis, tuned via `BoundaryCount`
  (default 1) and `MiddleCount` (default 5).
- Fully controlled state: bindable `CurrentPage` and `PageSize` with change callbacks.
- Theme-agnostic scoped CSS that inherits `currentColor` and exposes CSS custom properties, so it
  renders correctly in light mode, dark mode, or any custom theme without a stylesheet `<link>`.
- Responsive collapse driven by CSS container queries (no JavaScript); opt out with
  `DisableResponsive`.

## Usage

```razor
<Pager CurrentPage="page"
	   CurrentPageChanged="OnPageChanged"
	   PageSize="20"
	   TotalItems="totalItems"
	   ShowFirstLast="true"
	   ShowDescription="true"
	   ShowPageSizeSelector="true" />
```

Because the styles are scoped (isolated) CSS, no consumer `<link>` is required.

## Theming the page-size selector

The page-size `<select>` inherits the host's `color-scheme`, so an app that declares
`color-scheme: dark` at its root automatically gets a correctly themed native option flyout with no
extra work. For finer control, override these CSS custom properties (defaults preserve the current
theme-agnostic appearance):

| Variable | Default | Purpose |
| --- | --- | --- |
| `--pager-select-bg` | `transparent` (closed control), `canvas` (option flyout) | Background of the page-size selector and its options. |
| `--pager-select-color` | `inherit` (closed control), `canvastext` (option flyout) | Text color of the page-size selector and its options. |

```css
:root {
    --pager-select-bg: #2a2a3c;
    --pager-select-color: #e0e0e0;
}
```

No dependency on internal class names or `!important` is required.
