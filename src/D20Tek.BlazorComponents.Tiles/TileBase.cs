namespace D20Tek.BlazorComponents;

public abstract class TileBase : BaseComponent
{
    private bool _imageError;

    protected TileBase() => Size = Size.Medium;

    [Parameter]
    [EditorRequired]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string? Description { get; set; }

    [Parameter]
    public string? ImageUrl { get; set; }

    [Parameter]
    public string? Abbreviation { get; set; }

    [Parameter]
    public string? AbbreviationColor { get; set; }

    [Parameter]
    public LayoutOption Layout { get; set; } = LayoutOption.Common;

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> Clicked { get; set; }

    internal virtual string? ResolvedHref => null;

    internal virtual string? ResolvedTarget => null;

    internal bool HasImage => !string.IsNullOrWhiteSpace(ImageUrl) && !_imageError;

    internal bool HasFooter => Footer is not null;

    internal string Initials => TileAbbreviation.Compute(Title, Description, Abbreviation);

    internal string AbbreviationBackground =>
        string.IsNullOrWhiteSpace(AbbreviationColor)
            ? TileAbbreviationPalette.GetColor(AbbreviationKey)
            : AbbreviationColor;

    internal string AriaLabel => string.IsNullOrWhiteSpace(Title) ? Initials : Title;

    internal string? RootCssClass => CssClass;

    internal string? RootCssStyles => CssStyles;

    private string AbbreviationKey =>
        !string.IsNullOrWhiteSpace(Title) ? Title :
        !string.IsNullOrWhiteSpace(Description) ? Description :
        Initials;

    internal void OnImageError() => _imageError = true;

    internal async Task OnClickedAsync(MouseEventArgs args)
    {
        if (Clicked.HasDelegate)
        {
            await Clicked.InvokeAsync(args);
        }
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("tile")
            .AddClass(TileSizeMetadata.GetSizeCss(Size))
            .AddClass(TileLayoutMetadata.GetLayoutCss(Layout))
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
