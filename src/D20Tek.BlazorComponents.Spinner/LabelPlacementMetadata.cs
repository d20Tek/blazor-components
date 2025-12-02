namespace D20Tek.BlazorComponents;

internal static class LabelPlacementMetadata
{
    internal struct Item
    {
        public Placement Placement;
        public string CssClasses;
    }

    private const string _fixedCssPlacementTop = "spinner-label spinner-label-top";
    private const string _fixedCssPlacementBottom = "spinner-label spinner-label-bottom";
    private const string _fixedCssPlacementLeft = "spinner-label spinner-label-left";
    private const string _fixedCssPlacementRight = "spinner-label spinner-label-right";

    private static readonly List<Item> _elements =
    [
        new (){ Placement = Placement.Top, CssClasses = _fixedCssPlacementTop },
        new (){ Placement = Placement.Bottom, CssClasses = _fixedCssPlacementBottom},
        new (){ Placement = Placement.Left, CssClasses = _fixedCssPlacementLeft },
        new (){ Placement = Placement.Right, CssClasses = _fixedCssPlacementRight },
    ];

    public static string GetPlacementCss(Placement placement) =>
        _elements.First(p => p.Placement == placement).CssClasses;
}
