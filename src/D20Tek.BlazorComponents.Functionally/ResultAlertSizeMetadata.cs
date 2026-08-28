namespace D20Tek.BlazorComponents;

internal class ResultAlertSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "result-alert-xs" },
        { Size.Small, "result-alert-sm" },
        { Size.Medium, "result-alert-md" },
        { Size.Large, "result-alert-lg" },
        { Size.ExtraLarge, "result-alert-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
