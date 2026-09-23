using Microsoft.AspNetCore.Components.Web;

namespace D20Tek.BlazorComponents;

internal enum MenuKeyCommand
{
    None,
    Close,
    CloseAndReturnFocus,
    FocusNext,
    FocusPrevious,
    FocusFirst,
    FocusLast,
    TypeAhead
}

internal static class MenuKeyboard
{
    public static MenuKeyCommand Resolve(KeyboardEventArgs e) =>
        e.Key switch
        {
            "Escape" => MenuKeyCommand.CloseAndReturnFocus,
            "Tab" => MenuKeyCommand.Close,
            "ArrowDown" => MenuKeyCommand.FocusNext,
            "ArrowUp" => MenuKeyCommand.FocusPrevious,
            "Home" => MenuKeyCommand.FocusFirst,
            "End" => MenuKeyCommand.FocusLast,
            _ => ResolveDefault(e)
        };

    private static MenuKeyCommand ResolveDefault(KeyboardEventArgs e) =>
        e.Key.Length == 1 && !e.CtrlKey && !e.AltKey && !e.MetaKey
            ? MenuKeyCommand.TypeAhead
            : MenuKeyCommand.None;
}
