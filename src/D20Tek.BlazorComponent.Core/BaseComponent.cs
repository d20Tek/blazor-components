using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents;

public abstract class BaseComponent : ComponentBase
{
    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Parameter]
    public Size Size { get; set; } = Size.Small;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> RemainingAttributes { get; set; } = [];

    protected string? CssClass { get; set; } = null;

    protected string? CssStyles { get; set; } = null;

    protected override void OnParametersSet()
    {
        CssClass = CalculateCssClasses();
        CssStyles = CalculateCssStyles();
    }

    protected abstract string? CalculateCssClasses();

    protected abstract string? CalculateCssStyles();
}
