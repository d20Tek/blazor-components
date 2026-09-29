# D20Tek.BlazorComponents.Menus

A generic, reusable Blazor `FlyoutMenu` (popover) component. It is triggered from a built-in kebab
button or a fully custom trigger element, positions itself with collision-aware placement, and
supports both data-driven entries and templated child components with full keyboard accessibility.

## Features

- Self-positioning flyout triggered from a built-in kebab button or a custom `Trigger` fragment that
  receives the open state and a toggle callback.
- Data-driven entries (`MenuActionItem`, `MenuLinkItem`, `MenuSeparatorItem`, `MenuHeaderItem`) or
  templated child components (`MenuItem`, `MenuLink`, `MenuSeparator`, `MenuHeader`) that produce
  identical markup.
- Collision-aware placement (flip and shift) via a minimal isolated JavaScript module that portals
  the popup to `<body>`, with the geometry mirrored in a pure, unit-testable `PlacementCalculator`.
- Group-aware single-open policy: menus sharing a `GroupName` close one another when opened, while
  menus in different groups stay independent.
- Full keyboard accessibility (`aria-haspopup`, `aria-expanded`, `aria-controls`, `role="menu"`,
  `role="menuitem"`), Escape-to-close with focus return, arrow/Home/End navigation, and type-ahead.
- Density tiers via the shared `Size` parameter (`ExtraSmall` through `ExtraLarge`).
- Efficient theming through `--d20tek-menu-*` CSS custom properties with light defaults and an
  opt-in dark mode.

## Setup

The popup is portaled to `<body>` and its items are built in code, so a static CSS file must be
linked in your app's `wwwroot/index.html` (Blazor WASM) or `App.razor` / `_Host.cshtml`
(Blazor Server) inside the `<head>` tag:

```html
<link href="_content/D20Tek.BlazorComponents.Menus/Menu.css" rel="stylesheet" />
```

## Usage

Data-driven kebab menu:

```razor
<FlyoutMenu Items="_items" TriggerAriaLabel="Document actions" />

@code {
    private readonly IReadOnlyList<IMenuEntry> _items =
    [
        new MenuHeaderItem { Label = "Document" },
        new MenuActionItem { Label = "Rename", Icon = "oi oi-pencil", OnClick = OnRename },
        new MenuLinkItem { Label = "Open in new tab", Href = "/doc", Target = "_blank" },
        new MenuSeparatorItem(),
        new MenuActionItem { Label = "Delete", Icon = "oi oi-trash", IsDestructive = true },
    ];
}
```

Custom trigger:

```razor
<FlyoutMenu Items="_items">
    <Trigger Context="menu">
        <button @onclick="menu.Toggle">@(menu.IsOpen ? "Close" : "Open")</button>
    </Trigger>
</FlyoutMenu>
```

Templated children:

```razor
<FlyoutMenu>
    <MenuHeader Label="Document" />
    <MenuItem Label="Rename" Icon="oi oi-pencil" OnClick="OnRename" />
    <MenuLink Label="Open in new tab" Href="/doc" Target="_blank" />
    <MenuSeparator />
    <MenuItem Label="Delete" Icon="oi oi-trash" IsDestructive="true" />
</FlyoutMenu>
```

## Theming

The menu ships light defaults and exposes `--d20tek-menu-*` CSS custom properties. Declare overrides
on both the root and the popup (the popup is portaled to `<body>`, so it does not inherit variables
that are only defined on the root):

```css
.d20tek-flyout-menu,
.d20tek-flyout-menu__popup {
    --d20tek-menu-bg: #1e1e1e;
    --d20tek-menu-color: #e6e6e6;
    --d20tek-menu-border: 1px solid #333333;
}
```

Dark mode is opt-in: add a `data-theme="dark"`, `data-bs-theme="dark"`, or `.dark` marker to an
ancestor, or use `data-theme="auto"` to follow the OS `prefers-color-scheme`. Because the popup is
portaled to `<body>` when open, place the dark marker on `<html>` or `<body>`; if it must live on an
inner wrapper, add the `d20tek-menu--dark` class to the popup via the `MenuCssClass` parameter.

The derived hover/active/focus states use `color-mix`, which requires a modern browser
(Chrome/Edge 105+, Firefox 110+, Safari 16.2+).
