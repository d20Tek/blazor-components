namespace D20Tek.BlazorComponents;

internal static class MenuSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "d20tek-flyout-menu--xs" },
        { Size.Small, "d20tek-flyout-menu--sm" },
        { Size.Medium, "d20tek-flyout-menu--md" },
        { Size.Large, "d20tek-flyout-menu--lg" },
        { Size.ExtraLarge, "d20tek-flyout-menu--xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
