namespace D20Tek.BlazorComponents;

internal class TimerSizeMetadata
{
    private static Dictionary<Size, string> _elements = new()
    {
        { Size.None, String.Empty },
        { Size.ExtraSmall, "base-timer-xs" },
        { Size.Small, "base-timer-sm" },
        { Size.Medium, "base-timer-md" },
        { Size.Large, "base-timer-lg" },
        { Size.ExtraLarge, "base-timer-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
