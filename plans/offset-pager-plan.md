# Pagination Component Packages: Pager (core) + Vertically (OffsetPager<T>)

Design and implement two new component packages following the established
agnostic-core + framework-derived model already used for Toast (core) and
ResultToast<T> (in Functionally).

## Context & Conventions (from exploration)
- Solution: `blazor-components.slnx` uses one package per component under `src/`, all in namespace `D20Tek.BlazorComponents`, multi-target `net9.0;net10.0`, SDK `Microsoft.NET.Sdk.Razor`.
- All components inherit `BaseComponent` (in `D20Tek.BlazorComponent.Core`) with `IsVisible`, `Size`, `RemainingAttributes`, and abstract `CalculateCssClasses`/`CalculateCssStyles`. Use `CssBuilder`/`StyleBuilder`, `*SizeMetadata` static maps, and `ValueRange` for numeric validation.
- Reference model: `D20Tek.BlazorComponents.Toast` (core, references only `Core`); `D20Tek.BlazorComponents.Functionally` composes it. New packages mirror this: `Pager` references only `Core`; `Vertically` references `Pager` + the `D20Tek.Vertically` NuGet package.
- `D20Tek.Vertically 0.9.1` is already pinned in `Directory.Packages.props` (no project references it yet). The new `Vertically` package adds a `PackageReference` to it.
- Meta-package `D20Tek.BlazorComponents.All` bundles all component projects via `ProjectReference`.
- Tests: `D20Tek.BlazorComponents.UnitTests` (bUnit + MSTest). Follow user testing prefs: labeled Arrange/Act/Assert, `Method_State_Expectation` naming, one focused class per unit, split classes >200 lines, `Fakes` folder for test doubles, `Expected` markup constant partials, DataRow-driven metadata tests, aim for 100% block coverage.
- Docs: update README component/package tables, `CHANGELOG.md` (Keep a Changelog), `ReleaseNotes.md`, and any `docs/` api-reference. Use straight quotes and normal hyphens; no em/en-dashes.

## Design Decisions (confirmed with user)
- **Theming**: scoped CSS + `Size` metadata only; colors inherit `currentColor`/theme so light/dark/custom themes work automatically. No static `<link>` needed. Active/disabled/hover states use CSS custom properties (`--pager-active-bg`, etc.) with theme-neutral defaults consumers can override.
- **Default controls**: Prev/Next + numbered pages by default; First/Last, description ("page X of Y"), and page-size selector are all available but opt-in via boolean params.
- **State model**: core Pager is fully controlled (individual bound params + `EventCallback`s); an internal immutable `PageWindow` helper centralizes page-window + ellipsis math (no mutable public state bag).
- **Vertically integration**: `OffsetPager<T>` binds to the Vertically `OffsetPage<T>` (paged-result) type and raises a query/fetch `EventCallback`; it renders pager controls only (consumer renders the list).

## Page-Number Windowing (MudBlazor-style two-knob model)
Adopt the MudBlazor `MudPagination` approach (validated against MudBlazor + Blazorise research):
- **`BoundaryCount`** (int, default 1): number of pages always pinned at each end.
- **`MiddleCount`** (int, default 5): number of page buttons shown centered around the current page (current page plus two on each side by default).
- Anchored-ends-with-ellipsis output, e.g. 20 pages at page 10 with `BoundaryCount=1`, `MiddleCount=5` -> `1 ... 8 9 10 11 12 ... 20`.
- Ellipsis (`...`) inserted only when there is a real gap between the boundary group and the middle group; when the groups are adjacent they collapse (never render `1 ... 2 3`).
- Boundary de-duplication: never render a page number twice (e.g. when the middle window overlaps a boundary).
- Replaces the earlier single `PageWindowSize` param.

## Full-Featured Pager Anatomy (all opt-in fields ON)
Single root `<nav role="navigation" aria-label="pagination">`, flex row with three regions
(`justify-content: space-between`):
- **Left region - description** (opt-in `ShowDescription`): "Page X of Y".
- **Center region - control cluster** (left to right):
  - First (opt-in `ShowFirstLast`) - disabled on page 1
  - Prev (default) - disabled on page 1
  - Numbered pages with ellipsis (default `ShowNumbers`) using the two-knob window: `1 ... 8 9 10 11 12 ... 20`; current page has `aria-current="page"` + active class
  - Next (default) - disabled on last page
  - Last (opt-in `ShowFirstLast`) - disabled on last page
- **Right region - page-size selector** (opt-in `ShowPageSizeSelector`): labeled `<select>` bound to `PageSizeOptions` (e.g. "Rows: [20 v]").
- Each region is optional; when off it collapses and the flex layout rebalances.

## Responsive Collapse (container-query driven)
Root sets `container-type: inline-size`; `Pager.razor.css` uses `@container` breakpoints so the
Pager responds to its container width (works inside cards/panels), not just the viewport. Pure CSS,
no JS resize listeners or extra render passes. Progressive collapse (least essential dropped first):
1. Widest: everything shown.
2. Medium: hide description region.
3. Small: hide First/Last and page-size selector; keep numbers + Prev/Next.
4. Extra-small (mobile): also hide numbered pages; show only Prev / Next plus a compact
   `"X / Y"` indicator (a distinct element from the full description) so navigation still works.
- Param `DisableResponsive` (default `false`) opts out of auto-collapse for consumers managing layout themselves.
- Markup always renders all enabled regions; collapse is CSS-only. bUnit tests assert markup/classes and the presence of the responsive wrapper/classes and the compact indicator, rather than simulating container widths.

## Core Pager Public API (agnostic)
- Params: `CurrentPage` (int, 1-based, `ValueRange`-validated), `PageSize` (int), `TotalItems` (int) -> derive `TotalPages`; `BoundaryCount` (int, default 1), `MiddleCount` (int, default 5); toggles `ShowFirstLast`, `ShowDescription`, `ShowPageSizeSelector`, `ShowNumbers`; `PageSizeOptions` (IReadOnlyList<int>); `DisableResponsive` (bool, default false); `Size` (Core enum).
- EventCallbacks: `CurrentPageChanged`, `PageSizeChanged` (support `@bind-CurrentPage` / `@bind-PageSize`).
- Inherited: `IsVisible`, `RemainingAttributes`.
- Internal `PageWindow` (immutable struct/record): computes visible page numbers, leading/trailing ellipsis flags, boundary de-dup, and first/last/prev/next enabled states from CurrentPage/TotalPages/BoundaryCount/MiddleCount.
- `PagerSizeMetadata` static class mapping `Size` -> css modifier (per repo convention).

## OffsetPager<T> Public API (Vertically)
- Params: `Page` (Vertically `OffsetPage<T>` paged-result), `PageSizeOptions`, `BoundaryCount`/`MiddleCount`, control toggles + `DisableResponsive` + `Size` passthrough to core Pager.
- EventCallback `OnPageQuery` (or similarly named) raising the requested page index/size for the consumer to fetch the next `OffsetPage<T>`.
- Renders the core `Pager` wired from the `OffsetPage<T>` metadata (page number, size, total count); renders controls only, no item list.
- NOTE: confirm exact `OffsetPage<T>` property names against `D20Tek.Vertically 0.9.1` during implementation (total count, page index base, page size). Adjust binding accordingly.

## Risks / Open Items
- Exact `D20Tek.Vertically` type/member names (`OffsetPage<T>`, count/index/size properties, 0- vs 1-based) must be verified from the package at implementation time; the query callback signature depends on it.
- Windowing edge cases (few pages, current at start/end, single page, zero items, `MiddleCount`/`BoundaryCount` larger than total, overlap de-dup, adjacent-group ellipsis collapse) need explicit test coverage.
- Container-query support: baseline modern browsers support `@container`; document minimum browser expectations in the README.

## Steps
1. Create project `src/D20Tek.BlazorComponents.Pager/D20Tek.BlazorComponents.Pager.csproj` - `Microsoft.NET.Sdk.Razor`, mirror Toast csproj (Description, PackageTags, `SupportedPlatform browser`, `wwwroot` folder, `ProjectReference` to `D20Tek.BlazorComponent.Core`), add `GlobalUsings.cs` per convention.
2. Implement core Pager - `Pager.razor` (`@inherits BaseComponent`, `@if (IsVisible)` guard, `<nav role="navigation" aria-label="pagination">`, three regions with opt-in toggles, `@attributes`, `class`/`style` bindings, `aria-current` on active page, compact "X / Y" mobile indicator element), `Pager.razor.cs` (partial class with the params/EventCallbacks above incl. `BoundaryCount`/`MiddleCount` (default 5)/`DisableResponsive`, `CalculateCssClasses`/`CalculateCssStyles` via CssBuilder/StyleBuilder, `ValueRange` validation).
3. Implement `Pager.razor.css` - scoped, theme-agnostic using `currentColor` + CSS custom properties (`--pager-active-bg`, etc.); `container-type: inline-size` on root with `@container` breakpoints implementing the progressive collapse (description -> first/last + size selector -> numbers, leaving Prev/Next + compact indicator); size modifiers from metadata; responsive disabled when `DisableResponsive` class present.
4. Add internal helper types - `PageWindow` (immutable, MudBlazor-style two-knob windowing with `MiddleCount` default 5: computes pinned boundary pages + centered middle window, leading/trailing ellipsis flags, boundary de-dup, enabled states) and `PagerSizeMetadata` (static `Size`->css map).
5. Create project `src/D20Tek.BlazorComponents.Vertically/D20Tek.BlazorComponents.Vertically.csproj` - mirror Functionally csproj structure; add `PackageReference` to `D20Tek.Vertically` and `ProjectReference` to the new Pager project + `Core`; add `GlobalUsings.cs`.
6. Verify Vertically types - inspect `D20Tek.Vertically 0.9.1` `OffsetPage<T>` and related paged-result members to finalize binding and the query-callback signature.
7. Implement `OffsetPager<T>` - code-behind component composing the core `Pager`, binding from `OffsetPage<T>`, raising `OnPageQuery`, passing through toggles/`BoundaryCount`/`MiddleCount`/`DisableResponsive`/`Size`; renders pager controls only.
8. Register both projects in the solution - add `<Project>` entries to `blazor-components.slnx` under the `/Components/` folder.
9. Add both packages to the meta-package - add `ProjectReference`s to `src/D20Tek.BlazorComponents.All/D20Tek.BlazorComponents.All.csproj` and update its Description package list.
10. Add unit tests for the Pager package - in `tests/D20Tek.BlazorComponents.UnitTests`: `PagerTests` (rendering: default prev/next+numbers, all opt-in regions ON, opt-in first/last, description, page-size selector, size modifier, `IsVisible=false`, responsive wrapper/classes + compact indicator presence, `DisableResponsive`), split partials with `Expected` markup constants, behavior tests for `CurrentPageChanged`/`PageSizeChanged`, and `Fakes` if needed.
11. Add unit tests for helper types - `PageWindowTests` (two-knob windowing + ellipsis edge cases: default `MiddleCount=5` centered output, single page, zero items, current at start/middle/end, `MiddleCount`/`BoundaryCount` larger than total, overlap de-dup, adjacent-group ellipsis collapse) and DataRow-driven `PagerSizeMetadataTests`; follow AAA labeling and aim for 100% block coverage.
12. Add unit tests for `OffsetPager<T>` - bind an `OffsetPage<T>` fake/instance, assert core Pager is driven correctly and `OnPageQuery` fires with expected page/size; cover empty and populated pages.
13. Add sample page in `samples/D20Tek.FullSample.Wasm` - a new routable page demonstrating the core `Pager` (with toggles: first/last, description, page-size selector, `BoundaryCount`/`MiddleCount` tuning, and a resizable container showing the responsive collapse) and the `OffsetPager<T>` against sample `OffsetPage<T>` data; register it in the sample nav menu following existing sample pages.
14. Update docs - README component list + package table rows for Pager and Vertically (note scoped CSS/no `<link>`, two-knob windowing `BoundaryCount`/`MiddleCount` with defaults 1/5, container-query responsive behavior + min browser expectations), `CHANGELOG.md` Added entries, `ReleaseNotes.md` entry, and any `docs/` api-reference pages for the new public APIs.
15. Update CI/release publishing - add publish steps for both new packages to `.github/workflows/release.yml` (GitHub Packages + nuget.org), mirroring existing Toast entries.
16. Build and run tests - restore/build the solution for `net9.0;net10.0` and run the unit test project; fix any failures and verify coverage.
