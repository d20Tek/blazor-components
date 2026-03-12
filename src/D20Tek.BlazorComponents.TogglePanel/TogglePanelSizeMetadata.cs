namespace D20Tek.BlazorComponents;

internal class TogglePanelSizeMetadata
{
    private static Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "toggle-panel-xs" },
        { Size.Small, "toggle-panel-sm" },
        { Size.Medium, "toggle-panel-md" },
        { Size.Large, "toggle-panel-lg" },
        { Size.ExtraLarge, "toggle-panel-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
