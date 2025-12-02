namespace D20Tek.BlazorComponents;

internal class SpinnerSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, String.Empty },
        { Size.ExtraSmall, "1.25rem" },
        { Size.Small, "2rem" },
        { Size.Medium, "4rem" },
        { Size.Large, "8rem" },
        { Size.ExtraLarge, "12rem" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
