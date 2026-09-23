using Microsoft.AspNetCore.Components.Rendering;

namespace D20Tek.BlazorComponents;

public sealed class MenuLink : ComponentBase
{
    [CascadingParameter]
    private FlyoutMenu? Parent { get; set; }

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public RenderFragment? IconContent { get; set; }

    [Parameter]
    public bool IsDisabled { get; set; }

    [Parameter]
    public bool IsDestructive { get; set; }

    [Parameter]
    public bool CloseOnClick { get; set; } = true;

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public string Href { get; set; } = string.Empty;

    [Parameter]
    public string? Target { get; set; }

    [Parameter]
    public EventCallback OnClick { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var onClick = Parent is not null ? Parent.WrapClose(CloseOnClick, OnClick) : OnClick;
        MenuMarkup.RenderLink(builder, Label, Icon, IconContent, IsDisabled, IsDestructive, AriaLabel, Href, Target, onClick);
    }
}
