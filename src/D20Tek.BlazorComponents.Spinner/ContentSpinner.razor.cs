namespace D20Tek.BlazorComponents;

public partial class ContentSpinner : BaseComponent
{
    public ContentSpinner() => Size = Size.None;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private bool IsSizeRequired => Size != Size.None;

    protected override string? CalculateCssClasses() =>
        new CssBuilder(SpinnerConstants.ContentCssSpinnerMain)
                .AddClassFromAttributes(RemainingAttributes)
                .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .AddStyle(SpinnerConstants.StyleNameWidth, SpinnerSizeMetadata.GetSizeCss(Size), IsSizeRequired)
            .AddStyle(SpinnerConstants.StyleNameHeight, SpinnerSizeMetadata.GetSizeCss(Size), IsSizeRequired)
            .Build();
}
