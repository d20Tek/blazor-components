namespace D20Tek.BlazorComponents;

internal class ToggleSizeMetadata
{
    private static Dictionary<Size, string> _elements = new()
    {
        { Size.None, "toggle-md" },
        { Size.ExtraSmall, "toggle-xs" },
        { Size.Small, "toggle-sm" },
        { Size.Medium, "toggle-md" },
        { Size.Large, "toggle-lg" },
        { Size.ExtraLarge, "toggle-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
