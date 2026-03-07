# Markdown Component Implementation Plan

## Overview

Create a `MarkdownView` Blazor component in a new `D20Tek.BlazorComponents.Markdown` project that accepts a Markdown string, converts it to HTML using the [Markdig](https://github.com/xoofx/markdig) NuGet package, and renders it as safe HTML. The component must follow all existing solution design patterns described in `.github/copilot-instructions.md`.

---

## Solution Structure (after completion)

```
src/
??? D20Tek.BlazorComponents.Markdown/
    ??? D20Tek.BlazorComponents.Markdown.csproj
    ??? GlobalUsings.cs
    ??? _Imports.razor
    ??? IMarkdownRenderer.cs
    ??? MarkdigMarkdownRenderer.cs
    ??? MarkdownView.razor
    ??? MarkdownView.razor.cs
    ??? MarkdownView.razor.css
    ??? wwwroot/           (empty placeholder, matches other component projects)

tests/
??? D20Tek.BlazorComponents.UnitTests/
    ??? Markdown/
        ??? MarkdownViewTests.cs
        ??? MarkdownViewTests.Expected.cs
```

---

## Tasks

### Step 1 — Create the component project file

Create `src/D20Tek.BlazorComponents.Markdown/D20Tek.BlazorComponents.Markdown.csproj`.

- Use `Microsoft.NET.Sdk.Razor` SDK
- Target frameworks: `net9.0;net10.0`
- `RootNamespace`: `D20Tek.BlazorComponents`
- Enable `Nullable` and `ImplicitUsings`
- Standard package metadata (`Authors`, `Company`, `Version`, `Description`, etc.) matching other component projects
- Conditional `PackageReference` for `Microsoft.AspNetCore.Components.Web` (net9 ? 9.0.11, net10 ? 10.0.3)
- `PackageReference` for `Markdig` (latest stable, e.g. `0.40.0`) — **no conditional**, it targets `netstandard2.0` and works on both
- `SupportedPlatform` item for `browser`
- Empty `wwwroot\` folder item
- `ProjectReference` to `D20Tek.BlazorComponent.Core`

---

### Step 2 — Add `GlobalUsings.cs`

Create `src/D20Tek.BlazorComponents.Markdown/GlobalUsings.cs`.

```csharp
global using D20Tek.BlazorComponents.Utilities;
global using Microsoft.AspNetCore.Components;
global using System.Diagnostics.CodeAnalysis;
```

Matches the pattern of every other component project in the solution.

---

### Step 3 — Add `_Imports.razor`

Create `src/D20Tek.BlazorComponents.Markdown/_Imports.razor`.

```razor
@using Microsoft.AspNetCore.Components.Web
```

Matches the pattern of every other component project in the solution.

---

### Step 4 — Create `IMarkdownRenderer` interface

Create `src/D20Tek.BlazorComponents.Markdown/IMarkdownRenderer.cs`.

```csharp
namespace D20Tek.BlazorComponents;

public interface IMarkdownRenderer
{
    string ToHtml(string markdown);
}
```

Provides a seam for dependency injection and unit-test mocking.

---

### Step 5 — Create `MarkdigMarkdownRenderer` implementation

Create `src/D20Tek.BlazorComponents.Markdown/MarkdigMarkdownRenderer.cs`.

```csharp
using Markdig;

namespace D20Tek.BlazorComponents;

public class MarkdigMarkdownRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdigMarkdownRenderer()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    public string ToHtml(string markdown) =>
        Markdown.ToHtml(markdown ?? string.Empty, _pipeline);
}
```

Wraps Markdig with the `AdvancedExtensions` pipeline (tables, task lists, emoji, etc.).

---

### Step 6 — Create `MarkdownView.razor` markup file

Create `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor`.

```razor
@inherits BaseComponent

@if (this.IsVisible)
{
    <div @attributes="@this.RemainingAttributes" class="@this.CssClass" style="@this.CssStyles">
        @((MarkupString)_html)
    </div>
}
```

- Follows the same `@inherits BaseComponent` / `@if (this.IsVisible)` / `@attributes` pattern used by all other components.
- Wraps rendered HTML in a `<div>` so scoped CSS and class/style attributes can be applied.
- Uses `MarkupString` to inject raw HTML safely (Markdig output is sanitized by the pipeline configuration).

---

### Step 7 — Create `MarkdownView.razor.cs` code-behind

Create `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor.cs`.

```csharp
namespace D20Tek.BlazorComponents;

public partial class MarkdownView : BaseComponent
{
    private string _html = string.Empty;

    public MarkdownView() => Size = Size.None;

    [Parameter]
    public string Markdown { get; set; } = string.Empty;

    [Inject]
    private IMarkdownRenderer Renderer { get; set; } = default!;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _html = Renderer.ToHtml(Markdown);
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("markdown-view")
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
```

Key decisions:
- `Size = Size.None` in constructor (markdown body text size is controlled by consumer CSS, not the component).
- `OnParametersSet` re-renders HTML whenever parameters change, matching the reference code pattern.
- `IMarkdownRenderer` is `[Inject]`-ed privately — it's an implementation detail, not a public API.
- `CalculateCssClasses` applies a `"markdown-view"` base CSS class plus any attribute-splat classes.

---

### Step 8 — Create `MarkdownView.razor.css` scoped styles

Create `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor.css`.

Provide minimal base styles so the component renders sensibly out-of-the-box without overriding consumer styles. For example:

```css
.markdown-view {
    /* base container – consumers can override via class attribute splat */
}

.markdown-view :deep(pre) {
    overflow-x: auto;
}

.markdown-view :deep(img) {
    max-width: 100%;
}
```

---

### Step 9 — Add the new project to the solution

Run the `dotnet sln` command to register the new project:

```
dotnet sln add src/D20Tek.BlazorComponents.Markdown/D20Tek.BlazorComponents.Markdown.csproj
```

---

### Step 10 — Add project reference to the unit test project

Add a `ProjectReference` entry in `tests/D20Tek.BlazorComponents.UnitTests/D20Tek.BlazorComponents.UnitTests.csproj`:

```xml
<ProjectReference Include="..\..\src\D20Tek.BlazorComponents.Markdown\D20Tek.BlazorComponents.Markdown.csproj" />
```

---

### Step 11 — Create `MarkdownViewTests.Expected.cs` (partial class with expected HTML)

Create `tests/D20Tek.BlazorComponents.UnitTests/Markdown/MarkdownViewTests.Expected.cs`.

Use the partial-class + nested `Expected` static class pattern for storing multi-line HTML strings used across multiple test methods.

```csharp
namespace D20Tek.BlazorComponents.UnitTests.Markdown;

public partial class MarkdownViewTests
{
    internal static class Expected
    {
        public const string DefaultEmpty = @"<div class=""markdown-view""></div>";

        public const string Heading1 =
            @"<div class=""markdown-view""><h1 id=""hello-world"">Hello World</h1></div>";

        public const string BoldText =
            @"<div class=""markdown-view""><p><strong>bold</strong></p></div>";

        public const string WithCssClass =
            @"<div class=""markdown-view extra-class""></div>";
    }
}
```

> **Note:** The exact expected HTML strings must be verified by running the tests after implementation, as Markdig output (id attributes on headings, etc.) may differ slightly.

---

### Step 12 — Create `MarkdownViewTests.cs`

Create `tests/D20Tek.BlazorComponents.UnitTests/Markdown/MarkdownViewTests.cs`.

Test cases to implement:

| Test method | What it verifies |
|---|---|
| `DefaultRender_EmptyMarkdown` | Component with empty `Markdown` string renders wrapper div with no inner content |
| `Render_IsVisibleFalse` | `IsVisible = false` renders nothing (empty string) |
| `Render_Heading1` | `# Hello World` markdown produces an `<h1>` tag |
| `Render_BoldText` | `**bold**` produces `<strong>` tag |
| `Render_WithAttributeSplat` | Passing `class="extra-class"` via attribute splat appends to container div |
| `Render_NullMarkdown` | `Markdown = null` does not throw; renders empty wrapper div |
| `Render_MultilineMarkdown` | Paragraph + heading renders expected HTML blocks |
| `Renderer_ToHtml_CalledOnParametersSet` | Re-renders when `Markdown` parameter changes (SetParametersAsync) |

Each test registers a fake `IMarkdownRenderer` implementation via `ctx.Services.AddSingleton<IMarkdownRenderer>(...)` before rendering the component, keeping tests isolated from the real Markdig dependency.

Example structure:

```csharp
namespace D20Tek.BlazorComponents.UnitTests.Markdown;

[TestClass]
public partial class MarkdownViewTests
{
    private static BunitContext CreateContext(string renderedHtml = "")
    {
        var ctx = new BunitContext();
        var fakeRenderer = new FakeMarkdownRenderer(renderedHtml);
        ctx.Services.AddSingleton<IMarkdownRenderer>(fakeRenderer);
        return ctx;
    }

    [TestMethod]
    public void DefaultRender_EmptyMarkdown()
    {
        // arrange
        var ctx = CreateContext();

        // act
        var comp = ctx.Render<MarkdownView>();

        // assert
        comp.MarkupMatches(Expected.DefaultEmpty);
    }

    // ... additional test methods
}

internal class FakeMarkdownRenderer : IMarkdownRenderer
{
    private readonly string _output;
    public FakeMarkdownRenderer(string output) => _output = output;
    public string ToHtml(string markdown) => _output;
}
```

---

## Dependencies

| Package | Version | Project |
|---|---|---|
| `Markdig` | `0.40.0` (latest stable) | `D20Tek.BlazorComponents.Markdown` |
| `Microsoft.AspNetCore.Components.Web` | `9.0.11` / `10.0.3` (conditional) | `D20Tek.BlazorComponents.Markdown` |

> No new packages are needed in the test project — `bUnit` already handles Razor component rendering.

---

## Consumer Registration (for documentation / README)

Consumers must register `IMarkdownRenderer` in their DI container:

```csharp
// Program.cs
builder.Services.AddSingleton<IMarkdownRenderer, MarkdigMarkdownRenderer>();
```

And then use the component:

```razor
<MarkdownView Markdown="@markdownText" />
```

---

## Verification Steps

After implementation:

1. `dotnet build` — all projects build without errors
2. `dotnet test` — all existing tests pass, new Markdown tests pass
3. Manual smoke test in `D20Tek.FullSample.Wasm` sample app with `IMarkdownRenderer` registered and a `<MarkdownView>` page
