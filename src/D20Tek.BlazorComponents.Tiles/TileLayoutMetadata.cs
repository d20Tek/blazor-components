namespace D20Tek.BlazorComponents;

internal class TileLayoutMetadata
{
    private static readonly Dictionary<LayoutOption, string> _elements = new()
    {
        { LayoutOption.Compact, "tile-layout-compact" },
        { LayoutOption.Common, "tile-layout-common" },
        { LayoutOption.Verbose, "tile-layout-verbose" },
    };

    public static string GetLayoutCss(LayoutOption layout) => _elements[layout];
}
