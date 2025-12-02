namespace D20Tek.BlazorComponents;

public partial class Spinner : BaseComponent
{
    [Parameter]
    public SpinType Type { get; set; }

    [Parameter]
    public string Color { get; set; } = string.Empty;

    [Parameter]
    public string SecondaryColor { get; set; } = string.Empty;

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public Placement LabelPlacement { get; set; } = Placement.Bottom;

    private string? LabelCssClass { get; set; } = null;

    private bool HasLabel => !string.IsNullOrWhiteSpace(Label);

    private bool IsSizeRequired => (Size != Size.None && Size != Size.Small);

    private SpinTypeMetadata.Item TypeMetadata { get; set; } = SpinTypeMetadata.GetMetadataItem(SpinType.Ring);

    protected override string? CalculateCssClasses()
    {
        TypeMetadata = SpinTypeMetadata.GetMetadataItem(Type);
        if (HasLabel)
        {
            LabelCssClass = LabelPlacementMetadata.GetPlacementCss(LabelPlacement);
        }

        return new CssBuilder(TypeMetadata.FixedCssClass)
                        .AddClass(SpinnerConstants.FixedCssSpinnerMain, HasLabel)
                        .AddClassFromAttributes(RemainingAttributes)
                        .Build();
    }

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .AddStyle(SpinnerConstants.StyleNameColor, Color, () => string.IsNullOrWhiteSpace(Color) is false)
            .AddStyle(
                SpinnerConstants.StyleNameSecondaryColor,
                SecondaryColor,
                () => string.IsNullOrWhiteSpace(SecondaryColor) is false)
            .AddStyle(SpinnerConstants.StyleNameWidth, SpinnerSizeMetadata.GetSizeCss(Size), IsSizeRequired)
            .AddStyle(SpinnerConstants.StyleNameHeight, SpinnerSizeMetadata.GetSizeCss(Size), IsSizeRequired)
            .Build();
}
