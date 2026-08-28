# D20Tek.BlazorComponents.All

A convenience meta-package that bundles all of the D20Tek Blazor component packages into a
single reference. Install this if you want most or all of the components without adding a
separate `PackageReference` for each one.

> If you only need a few components, install the individual packages instead to keep your
> dependency graph minimal.

## Included packages

- `D20Tek.BlazorComponents.Functionally`
- `D20Tek.BlazorComponents.Markdown`
- `D20Tek.BlazorComponents.Modal`
- `D20Tek.BlazorComponents.Spinner`
- `D20Tek.BlazorComponents.Timer`
- `D20Tek.BlazorComponents.Toast`
- `D20Tek.BlazorComponents.TogglePanel`
- `D20Tek.BlazorComponents.ToggleSwitch`

Each of these depends transitively on `D20Tek.BlazorComponent.Core`, so it is included
automatically.

## Installation

```shell
dotnet add package D20Tek.BlazorComponents.All
```

This is a meta-package: it contains no assemblies of its own and simply references the
component packages above as NuGet dependencies.
