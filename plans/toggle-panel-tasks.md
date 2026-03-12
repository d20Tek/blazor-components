# TogglePanel Component Implementation Plan

## Overview
Create a `TogglePanel` component built entirely on the HTML5 `<details>` and `<summary>` elements with no JavaScript. This native browser behavior provides click-to-toggle and keyboard accessibility for free, making it ideal for accordions, filter panels, collapsible sections, and progressive disclosure patterns.

## Design Goals
- **Zero JavaScript** - Pure HTML5 `<details>`/`<summary>` elements
- **Accessible by default** - Keyboard navigation and screen reader support built in
- **Flexible content** - Header and body accept any Blazor `RenderFragment`
- **Two-way binding** - `@bind-IsOpen` support via `IsOpenChanged` callback
- **Consistent styling** - Follows the same CSS/class building patterns as all other components
- **Simple API** - Drop-in usage with minimal configuration

## HTML5 Foundation
```html
<!-- Native browser behavior, no JS required -->
<details open>
  <summary>Advanced Filters</summary>
  <label>
    <input type="checkbox" /> Show archived
  </label>
</details>
```
**Why it matters:**
- Click to toggle built in
- Keyboard accessible (Enter/Space on focused summary)
- No state management required in the browser
- No CSS hackery needed
- Full interactivity without JavaScript

## Component Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Summary` | `string` | `""` | Text displayed in the `<summary>` header |
| `SummaryContent` | `RenderFragment?` | `null` | Rich markup for summary header (takes precedence over `Summary`) |
| `ChildContent` | `RenderFragment?` | `null` | Content shown when panel is open |
| `IsOpen` | `bool` | `false` | Whether the panel starts expanded |
| `IsOpenChanged` | `EventCallback<bool>` | - | Enables `@bind-IsOpen` two-way binding |
| `OnToggle` | `EventCallback<bool>` | - | Fires when user toggles with current open state |
| `IsVisible` | `bool` | `true` | Inherited from `BaseComponent` |
| `Size` | `Size` | `Size.Medium` | Controls sizing of summary header text |
| `ShowChevron` | `bool` | `true` | Whether to show the right-justified SVG chevron indicator |
| `RemainingAttributes` | `Dictionary<string, object>` | `[]` | Captured unmatched attributes passed to `<details>` |

## Component Design

### Razor Markup Pattern
```razor
@inherits BaseComponent

@if (IsVisible)
{
    <details @attributes="RemainingAttributes"
             class="@CssClass"
             style="@CssStyles"
             open="@IsOpen"
             @ontoggle="HandleToggle">
        <summary class="toggle-panel__summary">
            <span class="toggle-panel__summary-text">
                @if (SummaryContent is not null)
                {
                    @SummaryContent
                }
                else
                {
                    @Summary
                }
            </span>
            @if (ShowChevron)
            {
                <span class="toggle-panel__chevron" aria-hidden="true">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16">
                        <path d="M6 9l6 6 6-6" stroke="currentColor" stroke-width="2" fill="none"
                              stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </span>
            }
        </summary>
        <div class="toggle-panel__body">
            @ChildContent
        </div>
    </details>
}
```

### Code-Behind Pattern
```csharp
public partial class TogglePanel : BaseComponent
{
    [Parameter] public string Summary { get; set; } = string.Empty;
    [Parameter] public RenderFragment? SummaryContent { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool IsOpen { get; set; } = false;
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }
    [Parameter] public EventCallback<bool> OnToggle { get; set; }
    [Parameter] public bool ShowChevron { get; set; } = true;

    private async Task HandleToggle(ToggleEventArgs args)
    {
        IsOpen = args.NewValue;
        await IsOpenChanged.InvokeAsync(IsOpen);
        await OnToggle.InvokeAsync(IsOpen);
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("toggle-panel")
            .AddClass(TogglePanelSizeMetadata.GetSizeCss(Size))
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
```

### Size Metadata
```csharp
internal class TogglePanelSizeMetadata
{
    private static Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "toggle-panel-xs" },
        { Size.Small, "toggle-panel-sm" },
        { Size.Medium, "toggle-panel-md" },
        { Size.Large, "toggle-panel-lg" },
        { Size.ExtraLarge, "toggle-panel-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
```

### CSS Approach
```css
/* Base panel styles */
.toggle-panel {
    border: 1px solid #e5e7eb;
    border-radius: 6px;
    overflow: hidden;
}

/* Summary (clickable header) */
.toggle-panel__summary {
    padding: 0.75rem 1rem;
    cursor: pointer;
    user-select: none;
    list-style: none; /* Remove default browser triangle */
    display: flex;
    align-items: center;
    justify-content: space-between;
    background-color: #f9fafb;
    font-weight: 500;
}

/* Remove default WebKit marker */
.toggle-panel__summary::-webkit-details-marker {
    display: none;
}

.toggle-panel__summary-text {
    flex: 1;
}

/* Right-justified SVG chevron - points down (closed), rotates up (open) */
.toggle-panel__chevron {
    display: flex;
    align-items: center;
    flex-shrink: 0;
    margin-left: 0.5rem;
    color: currentColor;
    transition: transform 0.2s ease;
}

details[open] .toggle-panel__chevron {
    transform: rotate(180deg);
}

/* Body content area */
.toggle-panel__body {
    padding: 1rem;
    border-top: 1px solid #e5e7eb;
}

/* Size variants - affect summary padding and font size */
.toggle-panel-sm .toggle-panel__summary { padding: 0.5rem 0.75rem; font-size: 0.875rem; }
.toggle-panel-md .toggle-panel__summary { padding: 0.75rem 1rem; font-size: 1rem; }
.toggle-panel-lg .toggle-panel__summary { padding: 1rem 1.25rem; font-size: 1.125rem; }
```

## Project Structure

```
src/D20Tek.BlazorComponents.TogglePanel/
├── D20Tek.BlazorComponents.TogglePanel.csproj
├── _Imports.razor
├── GlobalUsings.cs
├── TogglePanel.razor
├── TogglePanel.razor.cs
├── TogglePanel.razor.css
└── TogglePanelSizeMetadata.cs
```

## Usage Examples

### Basic Usage
```razor
<TogglePanel Summary="Advanced Filters">
    <label>
        <input type="checkbox" /> Show archived
    </label>
</TogglePanel>
```

### Open by Default
```razor
<TogglePanel Summary="Details" IsOpen="true">
    <p>This content is visible on first render.</p>
</TogglePanel>
```

### Two-Way Binding
```razor
<TogglePanel Summary="Settings" @bind-IsOpen="_settingsOpen">
    <p>Panel is @(_settingsOpen ? "open" : "closed")</p>
</TogglePanel>

@code {
    private bool _settingsOpen = false;
}
```

### Toggle Event Callback
```razor
<TogglePanel Summary="Notifications" OnToggle="HandleToggle">
    <p>Notification content here</p>
</TogglePanel>

@code {
    private void HandleToggle(bool isOpen) =>
        Console.WriteLine($"Panel is now {(isOpen ? "open" : "closed")}");
}
```

### Rich Summary Content
```razor
<TogglePanel>
    <SummaryContent>
        <span>⚙️ Advanced Options</span>
        <span class="badge">3 active</span>
    </SummaryContent>
    <ChildContent>
        <p>Custom content here.</p>
    </ChildContent>
</TogglePanel>
```

### Size Variants
```razor
<TogglePanel Summary="Small Panel" Size="Size.Small">
    <p>Compact content</p>
</TogglePanel>

<TogglePanel Summary="Large Panel" Size="Size.Large">
    <p>Spacious content</p>
</TogglePanel>
```

### Without Chevron
```razor
<TogglePanel Summary="Plain Header" ShowChevron="false">
    <p>No chevron indicator shown.</p>
</TogglePanel>
```

### Multiple Panels (Accordion-style)
```razor
<TogglePanel Summary="Section 1"><p>Content 1</p></TogglePanel>
<TogglePanel Summary="Section 2"><p>Content 2</p></TogglePanel>
<TogglePanel Summary="Section 3"><p>Content 3</p></TogglePanel>
```

## Testing Strategy

### Test Classes
Split tests across focused classes:

1. **TogglePanelTests.cs** - Core rendering (default, IsVisible, Summary, ChildContent)
2. **TogglePanelSizeTests.cs** - All Size variants apply correct CSS
3. **TogglePanelEventsTests.cs** - OnToggle, IsOpenChanged callbacks
4. **TogglePanelOpenTests.cs** - IsOpen default, open attribute rendering
5. **TogglePanelChevronTests.cs** - ShowChevron visibility and SVG rendering

### Key Test Cases
```csharp
// Default render
void DefaultRender()  // <details class="toggle-panel toggle-panel-md"> with no open attr

// IsOpen=true renders open attribute
void Render_WithIsOpenTrue()  // open="True" on <details>

// Size maps to correct CSS class
void Render_WithSizeSmall()  // toggle-panel-sm

// SummaryContent takes precedence over Summary string
void Render_WithSummaryContent_OverridesSummaryText()

// IsVisible=false renders nothing
void Render_IsVisibleFalse()  // MarkupMatches(string.Empty)

// OnToggle fires with current state
async Task OnToggle_FiresWhenSummaryClicked()

// @bind-IsOpen works via IsOpenChanged
async Task IsOpenChanged_FiresOnToggle()

// Chevron element is present by default
void Render_DefaultShowsChevron()  // .toggle-panel__chevron exists

// ShowChevron=false hides the chevron element
void Render_WithShowChevronFalse_HidesChevron()  // .toggle-panel__chevron absent
```

## Implementation Steps

### Step 1: Create TogglePanel project structure
- Add `D20Tek.BlazorComponents.TogglePanel.csproj` with `Microsoft.NET.Sdk.Razor`
- Multi-target `net9.0;net10.0`
- Add `GlobalUsings.cs`
- Add `_Imports.razor`

### Step 2: Create TogglePanelSizeMetadata class
- Map all `Size` enum values to CSS class names
- Follow the existing metadata pattern from other components

### Step 3: Create TogglePanel.razor markup
- Use `<details>` and `<summary>` HTML5 elements
- Support both `Summary` string and `SummaryContent` render fragment
- Apply `@attributes`, `class`, `style`, `open`, and `@ontoggle`
- Wrap summary text in `toggle-panel__summary-text` span
- Conditionally render right-justified SVG chevron span via `ShowChevron`
- Wrap `ChildContent` in `toggle-panel__body` div
- Guard with `@if (IsVisible)`

### Step 4: Create TogglePanel.razor.cs code-behind
- Inherit from `BaseComponent`
- Implement all parameters including `ShowChevron` (default `true`)
- Implement `HandleToggle` to fire both `IsOpenChanged` and `OnToggle`
- Implement `CalculateCssClasses` and `CalculateCssStyles`

### Step 5: Create TogglePanel.razor.css styles
- Base `.toggle-panel` container styles
- `.toggle-panel__summary` header styles (flex, `space-between`, hide browser marker)
- `.toggle-panel__summary-text` flex-grow span for summary label
- `.toggle-panel__chevron` right-justified SVG icon with `transition: transform`
- `details[open] .toggle-panel__chevron` rotates 180° when panel is open
- `.toggle-panel__body` content area styles
- All size variant CSS classes

### Step 6: Add project to solution
- Reference `D20Tek.BlazorComponent.Core`
- Add to solution file

### Step 7: Create unit tests - TogglePanelTests.cs
- `DefaultRender` - verifies default markup structure
- `Render_IsVisibleFalse` - verifies nothing renders
- `Render_WithSummary` - verifies summary text
- `Render_WithChildContent` - verifies body content
- `Render_WithSummaryContent` - verifies rich summary render fragment

### Step 8: Create unit tests - TogglePanelOpenTests.cs
- `Render_DefaultIsOpen_IsFalse` - no `open` attribute by default
- `Render_WithIsOpenTrue` - `open` attribute present
- `Render_IsOpen_DefaultValue` - checks instance.IsOpen is false

### Step 9: Create unit tests - TogglePanelSizeTests.cs
- One test per Size variant verifying correct CSS class applied

### Step 10: Create unit tests - TogglePanelEventsTests.cs
- `OnToggle_FiresWithCorrectValue`
- `IsOpenChanged_FiresOnToggle` - verifies two-way binding callback

### Step 11: Create unit tests - TogglePanelChevronTests.cs
- `Render_DefaultShowsChevron` - chevron element is rendered by default
- `Render_WithShowChevronFalse_HidesChevron` - no chevron element in markup
- `Render_Chevron_HasCorrectCssClass` - `.toggle-panel__chevron` class present
- `Render_Chevron_HasAriaHidden` - `aria-hidden="true"` on chevron span
- `Render_Chevron_ContainsSvg` - SVG element inside chevron span

### Step 12: Create sample page TogglePanelPage.razor
- Interactive parameter controls (Summary text, Size, IsOpen, IsVisible, ShowChevron)
- Live panel showing current state
- Examples section: basic, open-by-default, rich summary, no-chevron, multiple panels

### Step 13: Register sample page
- Add nav link to `NavMenu.razor`
- Add link to list in `Index.razor` page
- Add page to sample project

### Step 14: Build and verify all tests pass

## Files to Create

### Component Project
- `src/D20Tek.BlazorComponents.TogglePanel/D20Tek.BlazorComponents.TogglePanel.csproj`
- `src/D20Tek.BlazorComponents.TogglePanel/GlobalUsings.cs`
- `src/D20Tek.BlazorComponents.TogglePanel/_Imports.razor`
- `src/D20Tek.BlazorComponents.TogglePanel/TogglePanelSizeMetadata.cs`
- `src/D20Tek.BlazorComponents.TogglePanel/TogglePanel.razor`
- `src/D20Tek.BlazorComponents.TogglePanel/TogglePanel.razor.cs`
- `src/D20Tek.BlazorComponents.TogglePanel/TogglePanel.razor.css`

### Test Project
- `tests/D20Tek.BlazorComponents.UnitTests/TogglePanel/TogglePanelTests.cs`
- `tests/D20Tek.BlazorComponents.UnitTests/TogglePanel/TogglePanelOpenTests.cs`
- `tests/D20Tek.BlazorComponents.UnitTests/TogglePanel/TogglePanelSizeTests.cs`
- `tests/D20Tek.BlazorComponents.UnitTests/TogglePanel/TogglePanelEventsTests.cs`
- `tests/D20Tek.BlazorComponents.UnitTests/TogglePanel/TogglePanelChevronTests.cs`

### Sample Project
- `samples/D20Tek.FullSample.Wasm/Pages/TogglePanelPage.razor`
- `samples/D20Tek.FullSample.Wasm/Pages/TogglePanelPage.razor.cs`

## Files to Modify
- Solution file - Add new TogglePanel project reference
- `tests/D20Tek.BlazorComponents.UnitTests/D20Tek.BlazorComponents.UnitTests.csproj` - Add project reference
- `samples/D20Tek.FullSample.Wasm/Shared/NavMenu.razor` - Add nav link
