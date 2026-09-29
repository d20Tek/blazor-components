using Microsoft.AspNetCore.Components.Rendering;

namespace D20Tek.BlazorComponents;

internal static class MenuEntryRenderer
{
    public static void Render(RenderTreeBuilder builder, IMenuEntry entry, FlyoutMenu owner)
    {
        switch (entry)
        {
            case MenuLinkItem link:
                MenuMarkup.RenderLink(
                    builder, link.Label, link.Icon, link.IconContent, link.IsDisabled,
                    link.IsDestructive, link.AriaLabel, link.Href, link.Target,
                    owner.WrapClose(link.CloseOnClick, EventCallback.Empty));
                break;
            case MenuActionItem action:
                MenuMarkup.RenderAction(
                    builder, action.Label, action.Icon, action.IconContent, action.IsDisabled,
                    action.IsDestructive, action.AriaLabel,
                    owner.WrapClose(action.CloseOnClick, action.OnClick));
                break;
            case MenuHeaderItem header:
                MenuMarkup.RenderHeader(builder, header.Label);
                break;
            case MenuSeparatorItem:
                MenuMarkup.RenderSeparator(builder);
                break;
            default:
                break;
        }
    }
}
