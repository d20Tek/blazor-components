namespace D20Tek.BlazorComponents;

public partial class LinkTile : TileBase
{
    [Parameter]
    [EditorRequired]
    public string Href { get; set; } = string.Empty;

    [Parameter]
    public string? Target { get; set; }

    internal override string? ResolvedHref => Href;

    internal override string? ResolvedTarget => Target;
}
