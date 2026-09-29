namespace D20Tek.BlazorComponents;

internal static class Constants
{
    public const string CssFlyoutMenu = "d20tek-flyout-menu";
    public const string CssTrigger = "d20tek-flyout-menu__trigger";
    public const string CssPopup = "d20tek-flyout-menu__popup";
    public const string CssItem = "d20tek-flyout-menu__item";
    public const string CssSeparator = "d20tek-flyout-menu__separator";
    public const string CssHeader = "d20tek-flyout-menu__header";

    public const string DefaultTriggerAriaLabel = "More actions";
    public const string DefaultGroupName = "__d20tek_default_menu_group__";
    public const int DefaultOffset = 6;

    public static class JSFunctions
    {
        public const string Import = "import";
        public const string ModulePath = "./_content/D20Tek.BlazorComponents.Menus/Flyout/FlyoutMenu.razor.js";
        public const string Initialize = "initialize";
        public const string Reposition = "reposition";
        public const string Teardown = "teardown";
        public const string Dispose = "dispose";
        public const string MoveFocus = "moveFocus";
        public const string TypeAhead = "typeAhead";
    }
}
