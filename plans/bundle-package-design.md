# D20Tek.BlazorComponents.All (Meta-Package) Design

## Goal
Provide a single convenience "umbrella" NuGet package that pulls in all of the individual
D20Tek Blazor component packages, for users who want most or all of the components without
adding many separate `PackageReference` lines.

The individual component packages remain the primary, granular install path. The `.All`
package is an optional convenience meta-package.

## How it works (packaging behavior)
- A NuGet package does **not** physically merge referenced projects into itself.
- At `dotnet pack` time, each `<ProjectReference>` is converted into a NuGet **package
  dependency** in the produced `.nupkg` (not an embedded DLL).
- Installing `D20Tek.BlazorComponents.All` therefore transitively restores all referenced
  component packages. The bundle's own assembly is empty and is not shipped.
- This is the standard meta-package pattern (e.g., `Microsoft.AspNetCore.App`).

## Package name
- **`D20Tek.BlazorComponents.All`** — explicit and self-documenting ("everything" package).
- Keep the name plural (`...BlazorComponents...`) to match the component family.

## Project layout
- New project at `src\D20Tek.BlazorComponents.All\D20Tek.BlazorComponents.All.csproj`.
- Use plain **`Microsoft.NET.Sdk`** (not the Razor SDK) — there is no Razor content.
- No source files (optionally a single marker/doc file).
- Multi-target `net9.0;net10.0` to match the rest of the suite.

## Meta-package MSBuild settings
- `IncludeBuildOutput = false` — do not ship the empty assembly.
- `NoWarn = NU5128` — suppress the "no assembly in framework folder" meta-package warning.
- Standard package metadata: `Version` (from repo tag), `Description`, `PackageTags`,
  `Title` (e.g., "D20Tek Blazor Components (all)").
- Dependencies are kept when packing (default behavior).

## References
- Add a `<ProjectReference>` to each component package project:
  - Spinner, Modal, Timer, TogglePanel, ToggleSwitch, Markdown, ResultValidator.
- Do **not** reference `D20Tek.BlazorComponent.Core` directly — it flows in transitively
  through every component.
- Leave references with default pack behavior so they convert to package dependencies.

## Versioning (CPM + repo tagging)
- Repo tagging sets the same version for every package built/published in this repo.
- **No extra CPM changes required.** Central Package Management (`Directory.Packages.props`)
  only governs `PackageReference` versions. The bundle uses only `ProjectReference`s, which
  convert to package dependencies stamped with each referenced project's `<Version>`.
- Because the repo tag drives a uniform version across all packages, the bundle's own version
  and all of its dependency versions line up automatically.
- Constraint: do not add a `PackageReference` to the bundle without a matching central entry
  (it should not need any).

## Solution / CI integration
- Add the project to `blazor-components.slnx` under the `src` grouping.
- Include it in the pack/publish pipeline, produced and pushed alongside the component
  packages with the same tag-driven version.

## Documentation
- The `.All` package README/description should list every included component and note that
  users who need only a few components should install the individual packages instead.

## Assessment
- Good idea as an **optional convenience meta-package**.
- Pros: one-line install for the full suite; single place to document the library; users still
  get the individual packages underneath (no assembly duplication).
- Cons/considerations: pulls in all components even if unused (trimmable for WASM); keep the
  individual packages as the recommended granular path.
