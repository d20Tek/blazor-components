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

---
---

# Feature: Code Block Copy Button

## Overview

Add a **Copy** button to the top-right corner of every fenced code block rendered by `MarkdownView`. Clicking the button copies the code block's text content to the clipboard via `navigator.clipboard.writeText()`. Because the rendered HTML is injected via `MarkupString` (raw HTML), Blazor event handlers cannot be attached to elements inside it. A small **JavaScript ES module** with event delegation is the required approach — this is the standard pattern used by GitHub, VS Code preview, and documentation sites.

### Approach

Markdig outputs fenced code blocks as:

```html
<pre><code class="language-csharp">builder.Services.AddMarkdownRenderer();
</code></pre>
```

The feature has three layers:

1. **HTML post-processing** — After `Renderer.ToHtml()`, a `CodeBlockPostProcessor` wraps every `<pre><code>` block in a container `<div>` with an injected copy `<button>`:

```html
<div class="markdown-code-block">
    <button class="markdown-copy-btn" type="button" title="Copy code">
        <!-- clipboard SVG icon -->
    </button>
    <pre><code class="language-csharp">builder.Services.AddMarkdownRenderer();
</code></pre>
</div>
```

2. **JavaScript module** (`wwwroot/markdown-copy.js`) — Uses event delegation on the `MarkdownView` root element to listen for clicks on `.markdown-copy-btn` buttons. On click it reads the sibling `<pre><code>` text content, calls `navigator.clipboard.writeText()`, and briefly swaps the icon to a checkmark for visual feedback.

3. **Component integration** — `MarkdownView` loads the JS module via `IJSRuntime.InvokeAsync<IJSObjectReference>` in `OnAfterRenderAsync`, passes the component's `@ref` element for event delegation, and disposes the module reference via `IAsyncDisposable`.

### Why JavaScript is required

`MarkdownView` renders Markdig's HTML output as `@((MarkupString)_html)`. Blazor treats this as opaque HTML — no component tree is built, so `@onclick` handlers cannot be attached to elements inside it. The Clipboard API (`navigator.clipboard`) is also a browser API with no Blazor built-in equivalent. A JS module is the minimal, standard solution.

---

## Solution Structure (additions)

```
src/D20Tek.BlazorComponents.Markdown/
    ??? CodeBlockPostProcessor.cs          (new)
    ??? MarkdownView.razor                 (modify — add @ref)
    ??? MarkdownView.razor.cs              (modify — add ShowCopyButton, JS interop, IAsyncDisposable)
    ??? MarkdownView.razor.css             (modify — add code-block + copy-btn styles)
    ??? wwwroot/
        ??? markdown-copy.js               (new — ES module)

tests/D20Tek.BlazorComponents.UnitTests/
    ??? Markdown/
        ??? CodeBlockPostProcessorTests.cs (new)
        ??? MarkdownViewTests.cs           (modify — add ShowCopyButton tests)

samples/D20Tek.FullSample.Wasm/
    ??? Pages/
        ??? MarkdownViewPage.razor.cs      (modify — verify code block sample exists)
```

---

## Tasks

### Step 13 — Create `CodeBlockPostProcessor`

Create `src/D20Tek.BlazorComponents.Markdown/CodeBlockPostProcessor.cs`.

A static helper that post-processes the Markdig HTML string to wrap every `<pre><code...>` block in a container with a copy button.

```csharp
using System.Text.RegularExpressions;

namespace D20Tek.BlazorComponents;

internal static partial class CodeBlockPostProcessor
{
    private const string CopyButtonHtml =
        """<button class="markdown-copy-btn" type="button" title="Copy code"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg></button>""";

    public static string AddCopyButtons(string html) =>
        PreCodeRegex().Replace(html, $"""<div class="markdown-code-block">{CopyButtonHtml}$0""")
                      .Replace("</code></pre>", "</code></pre></div>");

    [GeneratedRegex(@"<pre><code(?:\s[^>]*)?>", RegexOptions.Compiled)]
    private static partial Regex PreCodeRegex();
}
```

Key decisions:
- Uses `[GeneratedRegex]` source generator for AOT-safe, allocation-efficient regex.
- The regex matches `<pre><code>` or `<pre><code class="language-xxx">` (Markdig's two output forms).
- The closing `</code></pre>` is always a literal string in Markdig output, so a simple `Replace` is safe.
- The SVG is a standard clipboard/copy icon (Feather Icons style).
- Class is `internal` — it's an implementation detail of the component.

---

### Step 14 — Create JS module `markdown-copy.js`

Create `src/D20Tek.BlazorComponents.Markdown/wwwroot/markdown-copy.js`.

An ES module that uses event delegation — one listener on the component root handles all copy buttons inside rendered Markdown.

```javascript
export function init(root) {
    if (root.__markdownCopyInit) return;
    root.__markdownCopyInit = true;

    root.addEventListener("click", async (e) => {
        const btn = e.target.closest(".markdown-copy-btn");
        if (!btn) return;

        const codeEl = btn.closest(".markdown-code-block")?.querySelector("pre > code");
        if (!codeEl) return;

        try {
            await navigator.clipboard.writeText(codeEl.textContent);
            btn.classList.add("copied");
            btn.setAttribute("title", "Copied!");
            setTimeout(() => {
                btn.classList.remove("copied");
                btn.setAttribute("title", "Copy code");
            }, 2000);
        } catch {
            // clipboard API not available or permission denied — fail silently
        }
    });
}

export function dispose(root) {
    // event listener cleanup not needed — it's removed when the element is removed from DOM
    if (root) root.__markdownCopyInit = false;
}
```

Key decisions:
- Uses `e.target.closest(".markdown-copy-btn")` so clicks on the SVG icon inside the button still work.
- The `__markdownCopyInit` guard prevents duplicate listeners if `init` is called multiple times.
- Visual "Copied!" feedback via CSS class swap + title attribute change, auto-reverts after 2 seconds.
- The module is loaded from the component's static assets path: `_content/D20Tek.BlazorComponents.Markdown/markdown-copy.js`.

---

### Step 15 — Add `ShowCopyButton` parameter to `MarkdownView.razor.cs`

Modify `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor.cs`.

- Add `[Parameter] public bool ShowCopyButton { get; set; } = true;` — opt-in by default.
- In `OnParametersSet`, conditionally post-process the HTML:

```csharp
protected override void OnParametersSet()
{
    base.OnParametersSet();
    _html = Renderer.ToHtml(Markdown);
    if (ShowCopyButton)
        _html = CodeBlockPostProcessor.AddCopyButtons(_html);
}
```

---

### Step 16 — Add JS interop and `IAsyncDisposable` to `MarkdownView.razor.cs`

Modify `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor.cs`.

- Implement `IAsyncDisposable`.
- Add a private `ElementReference _elementRef` field.
- Add a private `IJSObjectReference? _jsModule` field.
- Inject `IJSRuntime`.
- Override `OnAfterRenderAsync(bool firstRender)`:
  - On `firstRender` when `ShowCopyButton` is `true`, import the JS module and call `init(_elementRef)`.
- Implement `DisposeAsync` to call `dispose` and release the module reference.

```csharp
[Inject]
private IJSRuntime JsRuntime { get; set; } = default!;

private ElementReference _elementRef;
private IJSObjectReference? _jsModule;

protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender && ShowCopyButton)
    {
        _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/D20Tek.BlazorComponents.Markdown/markdown-copy.js");
        await _jsModule.InvokeVoidAsync("init", _elementRef);
    }
}

public async ValueTask DisposeAsync()
{
    if (_jsModule is not null)
    {
        await _jsModule.InvokeVoidAsync("dispose", _elementRef);
        await _jsModule.DisposeAsync();
    }
}
```

---

### Step 17 — Add `@ref` to `MarkdownView.razor`

Modify `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor`.

Add `@ref="_elementRef"` to the container `<div>`:

```razor
@inherits BaseComponent

@if (this.IsVisible)
{
    <div @ref="_elementRef"
         @attributes="@this.RemainingAttributes"
         class="@this.CssClass"
         style="@this.CssStyles">
        @((MarkupString)_html)
    </div>
}
```

---

### Step 18 — Add copy-button CSS to `MarkdownView.razor.css`

Modify `src/D20Tek.BlazorComponents.Markdown/MarkdownView.razor.css`.

Append styles for the code-block wrapper and copy button:

```css
.markdown-view :deep(.markdown-code-block) {
    position: relative;
}

.markdown-view :deep(.markdown-copy-btn) {
    position: absolute;
    top: 0.5rem;
    right: 0.5rem;
    padding: 0.25rem;
    border: 1px solid #d1d5db;
    border-radius: 4px;
    background: #f9fafb;
    color: #6b7280;
    cursor: pointer;
    opacity: 0;
    transition: opacity 0.15s ease;
    line-height: 1;
    display: flex;
    align-items: center;
}

.markdown-view :deep(.markdown-code-block:hover .markdown-copy-btn) {
    opacity: 1;
}

.markdown-view :deep(.markdown-copy-btn:hover) {
    background: #e5e7eb;
    color: #374151;
}

.markdown-view :deep(.markdown-copy-btn.copied) {
    opacity: 1;
    color: #16a34a;
    border-color: #16a34a;
}
```

Key decisions:
- Button is hidden (`opacity: 0`) until the code block is hovered — keeps the UI clean.
- `.copied` class (set by JS) shows a green color for the "Copied!" feedback state.
- Uses `:deep()` combinator because the HTML is rendered inside the component's scoped boundary but as raw `MarkupString` content.

---

### Step 19 — Create `CodeBlockPostProcessorTests`

Create `tests/D20Tek.BlazorComponents.UnitTests/Markdown/CodeBlockPostProcessorTests.cs`.

Test cases:

| Test method | What it verifies |
|---|---|
| `AddCopyButtons_WithCodeBlock_WrapsInContainer` | A single `<pre><code>` block gets wrapped in `.markdown-code-block` div with a `.markdown-copy-btn` button |
| `AddCopyButtons_WithLanguageClass_WrapsInContainer` | `<pre><code class="language-csharp">` is also matched and wrapped |
| `AddCopyButtons_NoCodeBlocks_ReturnsUnchanged` | HTML without `<pre><code>` is returned as-is |
| `AddCopyButtons_MultipleCodeBlocks_WrapsEach` | Multiple code blocks each get their own wrapper + button |
| `AddCopyButtons_EmptyString_ReturnsEmpty` | Empty string input returns empty string |

---

### Step 20 — Add `ShowCopyButton` tests to `MarkdownViewTests`

Modify `tests/D20Tek.BlazorComponents.UnitTests/Markdown/MarkdownViewTests.cs`.

Add test cases:

| Test method | What it verifies |
|---|---|
| `Render_ShowCopyButtonTrue_AddsCopyButtonToCodeBlock` | When `ShowCopyButton = true` (default) and HTML contains a code block, the rendered output includes `.markdown-copy-btn` |
| `Render_ShowCopyButtonFalse_NoCopyButton` | When `ShowCopyButton = false`, code blocks are rendered without the copy button wrapper |

---

### Step 21 — Verify sample page code block section

Verify `samples/D20Tek.FullSample.Wasm/Pages/MarkdownViewPage.razor` and `MarkdownViewPage.razor.cs`.

The existing `_codeMarkdown` field already contains fenced code blocks (```csharp and ```razor). Confirm the "Code Blocks" section on the sample page visually shows the copy button on hover and copies code to clipboard on click. No code changes expected unless the sample needs updating for the new `ShowCopyButton` parameter.

---

## Verification Steps (Copy Button feature)

After implementation:

1. `dotnet build` — all projects build without errors
2. `dotnet test` — all existing and new tests pass
3. Manual smoke test in `D20Tek.FullSample.Wasm`:
   - Navigate to **Markdown View** page
   - Hover over a fenced code block — copy button appears at top-right
   - Click the button — code is copied to clipboard, button shows "Copied!" feedback
   - `ShowCopyButton="false"` hides the button
4. Verify the live-preview textarea section also shows copy buttons on code blocks entered by the user
