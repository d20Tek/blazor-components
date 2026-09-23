namespace D20Tek.BlazorComponents;

internal static class MenuCss
{
    public static string? RootClasses(
        Size size, bool isOpen, IDictionary<string, object> attributes) =>
        new CssBuilder(Constants.CssFlyoutMenu)
            .AddClass(MenuSizeMetadata.GetSizeCss(size), size != Size.None)
            .AddClass("is-open", isOpen)
            .AddClassFromAttributes(attributes)
            .Build();

    public static string? RootStyles(IDictionary<string, object> attributes) =>
        new StyleBuilder().AddStyleFromAttributes(attributes).Build();

    public static string TriggerButtonClasses(bool isOpen, string? triggerCssClass) =>
        new CssBuilder(Constants.CssTrigger + "__button")
            .AddClass("is-open", isOpen)
            .AddClass(triggerCssClass!, !string.IsNullOrEmpty(triggerCssClass))
            .Build() ?? string.Empty;

    public static string PopupClasses(bool animate, string? menuCssClass) =>
        new CssBuilder(Constants.CssPopup)
            .AddClass(Constants.CssPopup + "--animate", animate)
            .AddClass(menuCssClass!, !string.IsNullOrEmpty(menuCssClass))
            .Build() ?? string.Empty;

    public static string? PopupStyle(int? zIndex) =>
        zIndex.HasValue
            ? new StyleBuilder()
                .AddStyle("--d20tek-menu-z-index", zIndex.Value.ToString())
                .Build()
            : null;
}
