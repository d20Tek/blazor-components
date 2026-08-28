[![CI/CD Build](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml)
[![Release](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml)
[![NuGet](https://img.shields.io/nuget/v/D20Tek.BlazorComponents.All.svg)](https://www.nuget.org/packages/D20Tek.BlazorComponents.All)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

# d20Tek Blazor-Components

## Introduction
This package suite provides custom, reusable Blazor components. These components are easy to use right out of the box, so developers can focus on building their applications. To keep the libraries small and independent, we have a project and NuGet package for each component, so that developers can just include what they need (and a huge component library).

**Live demo:** [components.d20tek.com](https://components.d20tek.com)

Supported components: Spinner, ContentSpinner, Timer, SpanTimer, CountdownTimer, ToggleSwitch, ModalDialog, ModalFormDialog, MessageBox, MarkdownView, TogglePanel, ResultValidator, ResultAlert, ResultView, Toast, and ResultToast.

Components ship grouped by package, so a single package can contain more than one component:

| NuGet package | Components |
| --- | --- |
| `D20Tek.BlazorComponents.Spinner` | Spinner, ContentSpinner |
| `D20Tek.BlazorComponents.Timer` | Timer, SpanTimer, CountdownTimer |
| `D20Tek.BlazorComponents.Toast` | Toast (ToastProvider) |
| `D20Tek.BlazorComponents.ToggleSwitch` | ToggleSwitch |
| `D20Tek.BlazorComponents.Modal` | ModalDialog, ModalFormDialog, MessageBox |
| `D20Tek.BlazorComponents.Markdown` | MarkdownView |
| `D20Tek.BlazorComponents.TogglePanel` | TogglePanel |
| `D20Tek.BlazorComponents.Functionally` | ResultValidator, ResultAlert, ResultView, ResultToast |

### The "All" meta-package
If you would rather not reference each component package individually, install the **D20Tek.BlazorComponents.All** meta-package. It is a convenience bundle that transitively references the full component suite (Functionally, Markdown, Modal, Spinner, Timer, Toast, TogglePanel, and ToggleSwitch) through a single `PackageReference`. It ships no assemblies of its own, so you get exactly the same components as installing them one-by-one. If you only need a few components, install the individual p

> Note: because the meta-package includes the Modal and Toast components, apps that use `All` still need to link their static CSS files - see [Component-Specific Setup](#component-specific-setup) below.

## Installation
These libraries are in NuGet packages so they are easy to add to your project. To install these packages into your solution, you can use the Package Manager. In PM, please use the following commands:
> Tip: omit the `-Version` argument to install the latest published version of any package.
```  
PM > Install-Package D20Tek.BlazorComponents.Spinner -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.Timer -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.Toast -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.ToggleSwitch -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.Modal -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.Markdown -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.TogglePanel -Version 1.11.1
PM > Install-Package D20Tek.BlazorComponents.Functionally -Version 1.11.1
``` 

Or install everything at once with the meta-package:
```  
PM > Install-Package D20Tek.BlazorComponents.All -Version 1.11.1
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
> Note: `ResultToast` (in the `D20Tek.BlazorComponents.Functionally` package) is built on top of Toast. When you install `Functionally`, the `D20Tek.BlazorComponents.Toast` package comes with it transitively, so no extra package reference is needed - but you still need the setup above: link `Toast.css`, add `<ToastProvider />` to your layout, and call `builder.Services.AddToast();`.

### Samples:
For more detailed examples on how to use the D20Tek.BlazorComponents libraries, please review the following samples:

* [Sample - D20Tek.FullSample.Wasm](samples/D20Tek.FullSample.Wasm)
See this sample app running live at https://components.d20tek.com.

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository.

## License
This project is licensed under the [MIT License](LICENSE).
