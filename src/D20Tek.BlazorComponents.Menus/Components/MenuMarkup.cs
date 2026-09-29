using Microsoft.AspNetCore.Components.Rendering;

namespace D20Tek.BlazorComponents;

internal static class MenuMarkup
{
    public static void RenderAction(
        RenderTreeBuilder builder, string label, string? icon, RenderFragment? iconContent, bool isDisabled, 
        bool isDestructive, string? ariaLabel, EventCallback onClick)
    {
        builder.OpenElement(0, "button");
        builder.AddAttribute(1, "type", "button");
        builder.AddAttribute(2, "role", "menuitem");
        builder.AddAttribute(3, "class", ItemCss(isDestructive, isDisabled));
        builder.AddAttribute(4, "tabindex", "-1");
        if (isDisabled)
        {
            builder.AddAttribute(5, "disabled", true);
            builder.AddAttribute(6, "aria-disabled", "true");
        }

        if (!string.IsNullOrEmpty(ariaLabel))
        {
            builder.AddAttribute(7, "aria-label", ariaLabel);
        }

        if (!isDisabled)
        {
            builder.AddAttribute(8, "onclick", onClick);
        }

        RenderInner(builder, 9, label, icon, iconContent);
        builder.CloseElement();
    }

    public static void RenderLink(
        RenderTreeBuilder builder, string label, string? icon, RenderFragment? iconContent, bool isDisabled, 
        bool isDestructive, string? ariaLabel, string href, string? target, EventCallback onClick)
    {
        builder.OpenElement(0, "a");
        builder.AddAttribute(1, "role", "menuitem");
        builder.AddAttribute(2, "class", ItemCss(isDestructive, isDisabled));
        builder.AddAttribute(3, "tabindex", "-1");

        if (isDisabled)
        {
            builder.AddAttribute(4, "aria-disabled", "true");
        }
        else
        {
            builder.AddAttribute(5, "href", href);
            if (!string.IsNullOrEmpty(target))
            {
                builder.AddAttribute(6, "target", target);
                if (string.Equals(target, "_blank", StringComparison.OrdinalIgnoreCase))
                {
                    builder.AddAttribute(7, "rel", "noopener noreferrer");
                }
            }

            builder.AddAttribute(8, "onclick", onClick);
        }

        if (!string.IsNullOrEmpty(ariaLabel))
        {
            builder.AddAttribute(9, "aria-label", ariaLabel);
        }

        RenderInner(builder, 10, label, icon, iconContent);
        builder.CloseElement();
    }

    public static void RenderSeparator(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "role", "separator");
        builder.AddAttribute(2, "class", Constants.CssSeparator);
        builder.CloseElement();
    }

    public static void RenderHeader(RenderTreeBuilder builder, string label)
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "role", "group");
        builder.AddAttribute(2, "class", Constants.CssHeader);
        builder.AddAttribute(3, "aria-label", label);
        builder.AddContent(4, label);
        builder.CloseElement();
    }

    private static void RenderInner(
        RenderTreeBuilder builder, int seq, string label, string? icon, RenderFragment? iconContent)
    {
        if (iconContent is not null)
        {
            builder.OpenElement(seq, "span");
            builder.AddAttribute(seq + 1, "class", $"{Constants.CssItem}-icon");
            builder.AddAttribute(seq + 2, "aria-hidden", "true");
            builder.AddContent(seq + 3, iconContent);
            builder.CloseElement();
        }
        else if (!string.IsNullOrEmpty(icon))
        {
            builder.OpenElement(seq + 4, "i");
            builder.AddAttribute(seq + 5, "class", $"{Constants.CssItem}-icon {icon}");
            builder.AddAttribute(seq + 6, "aria-hidden", "true");
            builder.CloseElement();
        }

        builder.OpenElement(seq + 7, "span");
        builder.AddAttribute(seq + 8, "class", $"{Constants.CssItem}-label");
        builder.AddContent(seq + 9, label);
        builder.CloseElement();
    }

    private static string ItemCss(bool isDestructive, bool isDisabled) =>
        (isDestructive, isDisabled) switch
        {
            (true, true) => $"{Constants.CssItem} {Constants.CssItem}--destructive {Constants.CssItem}--disabled",
            (true, false) => $"{Constants.CssItem} {Constants.CssItem}--destructive",
            (false, true) => $"{Constants.CssItem} {Constants.CssItem}--disabled",
            (false, false) => Constants.CssItem,
        };
}
