namespace D20Tek.BlazorComponents;

internal class PagerSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "pager-xs" },
        { Size.Small, "pager-sm" },
        { Size.Medium, "pager-md" },
        { Size.Large, "pager-lg" },
        { Size.ExtraLarge, "pager-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
