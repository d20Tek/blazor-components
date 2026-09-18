[![CI/CD Build](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml)
[![Release](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/D20Tek.BlazorComponents.All.svg)](https://www.nuget.org/packages/D20Tek.BlazorComponents.All)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

# d20Tek Blazor-Components

## Introduction
This package suite provides custom, reusable Blazor components. These components are easy to use right out of the box, so developers can focus on building their applications. To keep the libraries small and independent, we have a project and NuGet package for each component, so that developers can just include what they need (and a huge component library).

**Live demo:** [components.d20tek.com](https://components.d20tek.com)

Supported components: Spinner, ContentSpinner, Timer, SpanTimer, CountdownTimer, Tile, LinkTile, ToggleSwitch, ModalDialog, ModalFormDialog, MessageBox, MarkdownView, TogglePanel, ResultValidator, ResultAlert, ResultView, Toast, ResultToast, Pager, and OffsetPager.

Components ship grouped by package, so a single package can contain more than one component:

| NuGet package | Components |
| --- | --- |
| `D20Tek.BlazorComponents.Spinner` | Spinner, ContentSpinner |
| `D20Tek.BlazorComponents.Timer` | Timer, SpanTimer, CountdownTimer |
| `D20Tek.BlazorComponents.Tiles` | Tile, LinkTile (isolated CSS, no link needed) |
| `D20Tek.BlazorComponents.Toast` | Toast (ToastProvider) |
| `D20Tek.BlazorComponents.ToggleSwitch` | ToggleSwitch |
| `D20Tek.BlazorComponents.Modal` | ModalDialog, ModalFormDialog, MessageBox |
| `D20Tek.BlazorComponents.Markdown` | MarkdownView |
| `D20Tek.BlazorComponents.TogglePanel` | TogglePanel |
| `D20Tek.BlazorComponents.Functionally` | ResultValidator, ResultAlert, ResultView, ResultToast |
| `D20Tek.BlazorComponents.Pager` | Pager |
| `D20Tek.BlazorComponents.Vertically` | OffsetPager |

### The "All" meta-package
references the full component suite (Functionally, Markdown, Modal, Pager, Spinner, Tiles, Timer, Toast, TogglePanel, ToggleSwitch, and Vertically) through a single `PackageReference`.

> Note: because the meta-package includes the Modal and Toast components, apps that use `All` still need to link their static CSS files - see [Component-Specific Setup](#component-specific-setup) below.

## Installation
These libraries are in NuGet packages so they are easy to add to your project. To install these packages into your solution, you can use the Package Manager. In PM, please use the following commands:
> Tip: omit the `-Version` argument to install the latest published version of any package.
```  
PM > Install-Package D20Tek.BlazorComponents.Spinner -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Timer -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Toast -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.ToggleSwitch -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Modal -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Markdown -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.TogglePanel -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Functionally -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Pager -Version 1.11.16
PM > Install-Package D20Tek.BlazorComponents.Vertically -Version 1.11.16
``` 

Or install everything at once with the meta-package:
```  
PM > Install-Package D20Tek.BlazorComponents.All -Version 1.11.16
``` 

To install in the Visual Studio UI, go to the Tools menu > "Manage NuGet Packages". Then search for D20Tek.BlazorComponents.Spinner and install it from there.

Read more about this release in our [Release Notes](ReleaseNotes.md).

## Usage
Once you've installed the component NuGet package, you can start using it in your Blazor project. For this example we will use the Spinner component, but other components will follow the same usage pattern.

1. You must add our namespace to your Blazor project's ```_Imports.razor``` file to make our types available to all of your pages/components.
```
@using D20Tek.BlazorComponents
```
2. Place the component in your razor file (for example in your Index.Razor file):
```
<Spinner />
```
3. Many components have additional parameters that you can call (see the component documentation for list of available parameters).
```
<Spinner Type=SpinType.Pulse Label="Loading..." />
```
4. Further customization is available by defining your own CSS or styles on the component.
```
<Spinner Type=SpinType.Hourglass class="my-custom-spinner" style="color: red; height: 120px; width: 120px" />
```
5. The ContentSpinner component allows you to spin any child content, like an image.
```
<ContentSpinner Size=Size.Medium>
    <img src="./images/my-image.png" style="width: 100%; height: 100%" />
</ContentSpinner>
```

### Component-Specific Setup

Some components require a one-time setup step in addition to the standard usage above.

**ModalDialog / ModalFormDialog / MessageBox** - These components use a static CSS file that must be linked in your app's `wwwroot/index.html` (Blazor WASM) or `App.razor` / `_Host.cshtml` (Blazor Server) inside the `<head>` tag:
```html
<link href="_content/D20Tek.BlazorComponents.Modal/Modal.css" rel="stylesheet" />
```

**Toast / ResultToast** - The Toast package uses a static CSS file that must be linked inside the `<head>` tag, and the `<ToastProvider />` component must be placed once in your layout (e.g. `MainLayout.razor`). Register the service with `builder.Services.AddToast();`:
```html
<link href="_content/D20Tek.BlazorComponents.Toast/Toast.css" rel="stylesheet" />
```
You can optionally configure app-wide default presentation settings that apply to every toast (both generic `Show` and `ResultToast.ShowResult`):
```csharp
builder.Services.AddToast(options =>
{
    options.Position = ToastPosition.TopRight;
    options.DefaultTimeout = TimeSpan.FromSeconds(4);
    options.ShowIcon = true;
    options.Dismissible = true;
    options.Animate = true;
});
```
These defaults cover presentation chrome only (`Position`, `DefaultTimeout`, `ShowIcon`, `Dismissible`, `Animate`). Values resolve in the order: **component built-in defaults -> app defaults (`AddToast`) -> per-call `configure`** on `Show`/`ShowResult`, so any individual call can still override the app defaults. Note that `DefaultTimeout` seeds the timeout for generic toasts and for `ResultToast` success toasts; `ResultToast` failure toasts remain sticky by default.
> Note: `ResultToast` (in the `D20Tek.BlazorComponents.Functionally` package) is built on top of Toast. When you install `Functionally`, the `D20Tek.BlazorComponents.Toast` package comes with it transitively, so no extra package reference is needed - but you still need the setup above: link `Toast.css`, add `<ToastProvider />` to your layout, and call `builder.Services.AddToast();`.

**Pager / OffsetPager** - The `Pager` component (in `D20Tek.BlazorComponents.Pager`) uses scoped CSS, so there is no static stylesheet to link. Its styles derive from `currentColor`, allowing it to adapt to any light, dark, or custom theme. `Pager` is fully controlled: bind `CurrentPage`/`CurrentPageChanged`, `PageSize`/`PageSizeChanged`, and `TotalItems`. Numbered pages use a MudBlazor-style anchored window controlled by two knobs, `BoundaryCount` (default 1) and `MiddleCount` (default 5, centered on the current page). The pager collapses its subcomponents (description, first/last, page-size selector, then numbers) as its container narrows using CSS container queries; set `DisableResponsive="true"` to opt out. Container queries require a modern browser (Chrome/Edge 105+, Firefox 110+, Safari 16+).
>
> `OffsetPager<T>` (in `D20Tek.BlazorComponents.Vertically`) wraps `Pager` for the D20Tek.Vertically paging types. Bind its `Page` parameter to a `PageOf<T>` result and handle `OnPageQuery`, which raises a new `PagedRequest` (one-based `PageNumber` plus `PageSize`) whenever the user navigates or changes the page size.

### Samples:
For more detailed examples on how to use the D20Tek.BlazorComponents libraries, please review the following samples:

* [Sample - D20Tek.FullSample.Wasm](samples/D20Tek.FullSample.Wasm)
See this sample app running live at https://components.d20tek.com.

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository.

## License
This project is licensed under the [MIT License](LICENSE).
