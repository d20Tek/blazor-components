# d20Tek Blazor-Components
[![Release](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/release.yml)
[![CI/CD Build](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml/badge.svg)](https://github.com/d20Tek/blazor-components/actions/workflows/blazor-components-ci.yml)

## Introduction
This package suite provides custom, resuable Blazor components. These components are easy to use right out of the box, so developers can focus on building their applications. To keep the libraries small and independent, we have a project and NuGet package for each component, so that developers can just include what they need (and a huge component library).

Supported components: Spinner, SpanSpinner (duration defined with TimeSpan).
Future components: ImageSpinner, Timer.

## Installation
These libraries are in NuGet packages so they are easy to add to your project. To install these packages into your solution, you can use the Package Manager. In PM, please use the following commands:
```  
PM > Install-Package D20Tek.BlazorComponents.Spinner -Version 1.0.11
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

### Samples:
For more detailed examples on how to use the D20Tek.BlazorComponents libraries, please review the following samples:

* [Sample - D20Tek.FullSample.Wasm](samples/D20Tek.FullSample.Wasm)

## Feedback
If you use these libraries and have any feedback, bugs, or suggestions, please file them in the Issues section of this repository.
