using Microsoft.AspNetCore.Components.Rendering;

namespace D20Tek.BlazorComponents;

public sealed class MenuHeader : ComponentBase
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    protected override void BuildRenderTree(RenderTreeBuilder builder) => MenuMarkup.RenderHeader(builder, Label);
}
