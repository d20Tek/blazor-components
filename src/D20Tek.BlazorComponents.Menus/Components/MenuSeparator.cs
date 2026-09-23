using Microsoft.AspNetCore.Components.Rendering;

namespace D20Tek.BlazorComponents;

public sealed class MenuSeparator : ComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder) => MenuMarkup.RenderSeparator(builder);
}
