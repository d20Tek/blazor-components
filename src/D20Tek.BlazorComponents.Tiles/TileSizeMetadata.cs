namespace D20Tek.BlazorComponents;

internal class TileSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "tile-xs" },
        { Size.Small, "tile-sm" },
        { Size.Medium, "tile-md" },
        { Size.Large, "tile-lg" },
        { Size.ExtraLarge, "tile-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
