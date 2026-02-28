# Copilot Instructions

## Solution Architecture
- **Core library**: `D20Tek.BlazorComponent.Core` - Contains `BaseComponent`, enums (`Size`, `Placement`), utilities (`CssBuilder`, `StyleBuilder`, `ValueRange`), shared across all components
- **Component libraries**: Each component in separate project (e.g., `D20Tek.BlazorComponents.Spinner`, `D20Tek.BlazorComponents.Timer`)
- **Test project**: `D20Tek.BlazorComponents.UnitTests` uses **bUnit** with **MSTest**
- **Root namespace**: All components use `D20Tek.BlazorComponents` namespace
- **Target frameworks**: Multi-target `net9.0;net10.0`
- **SDK**: Component projects use `Microsoft.NET.Sdk.Razor`

## Component Design Patterns

### 1. BaseComponent Inheritance
All components inherit from `BaseComponent`:public abstract class BaseComponent : ComponentBase {
    [Parameter] public bool IsVisible { get; set; } = true;
    [Parameter] public Size Size { get; set; } = Size.Small;
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> RemainingAttributes { get; set; } = [];
    protected string? CssClass { get; set; }
    protected string? CssStyles { get; set; }
    protected abstract string? CalculateCssClasses();
    protected abstract string? CalculateCssStyles();
}

### 2. Component File Structure
Each component has:
- `ComponentName.razor` - Markup with `@inherits` directive
- `ComponentName.razor.cs` - Code-behind with `partial class`
- `ComponentName.razor.css` - Scoped CSS styles

### 3. CSS/Style Building Pattern
Use fluent builders for CSS classes and inline styles:protected override string? CalculateCssClasses() =>
    new CssBuilder("base-css-class")
        .AddClass(SizeMetadata.GetSizeCss(Size))
        .AddClassFromAttributes(RemainingAttributes)
        .Build();

protected override string? CalculateCssStyles() =>
    new StyleBuilder()
        .AddStyleFromAttributes(RemainingAttributes)
        .AddStyle("property", "value", condition)
        .Build();

### 4. Metadata Classes Pattern
Use static metadata classes for Size/Type mappings:internal class ComponentSizeMetadata {
    private static Dictionary<Size, string> _elements = new() {
        { Size.None, string.Empty },
        { Size.Small, "component-sm" },
        { Size.Medium, "component-md" },
        // ...
    };
    public static string GetSizeCss(Size size) => _elements[size];
}

### 5. Parameter Validation
Use `ValueRange` for validating numeric parameters:protected static readonly ValueRange _validRange = new(0, 1000);
[Parameter]
[SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Needed")]
public int Value {
    get => _value;
    set { _validRange.AssertInRange(value, nameof(Value)); _value = value; }
}

### 6. Razor Markup Pattern@inherits BaseComponent
@if (IsVisible) {
    <div @attributes="@this.RemainingAttributes" role="..." class="@this.CssClass" style="@this.CssStyles">
        <!-- content -->
    </div>
}

### 7. GlobalUsings.cs
Each component project includes:global using D20Tek.BlazorComponents.Utilities;
global using Microsoft.AspNetCore.Components;
global using System.Diagnostics.CodeAnalysis;

## Unit Testing Patterns

### 1. Test Framework
- **bUnit** with `BunitContext` (not `TestContext`)
- **MSTest** attributes: `[TestClass]`, `[TestMethod]`

### 2. Test Structure (AAA Pattern)[TestMethod]
public void TestName() {
    // arrange
    var ctx = new BunitContext();

    // act
    var comp = ctx.Render<ComponentName>(parameters =>
        parameters.Add(p => p.Property, value));

    // assert
    var expectedHtml = @"<div>...</div>";
    comp.MarkupMatches(expectedHtml);
    Assert.AreEqual(expectedValue, comp.Instance.Property);
}

### 3. Expected Markup Pattern (for complex components)
Use partial class with nested `Expected` class for complex HTML:public partial class ComponentTests {
    internal static class Expected {
        public const string Default = @"<div>...</div>";
        public const string WithParameters = @"<div>...</div>";
    }
}

### 4. Custom Verifiers
Create helper classes for reusable assertion logic:internal static class ComponentVerifier {
    internal static void VerifyMarkupDifferences(IReadOnlyList<IDiff> results) {
        // Custom verification logic
    }
}

### 5. GlobalUsings for Testsglobal using Bunit;
global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using System;
global using System.Collections.Generic;
global using System.Diagnostics.CodeAnalysis;
