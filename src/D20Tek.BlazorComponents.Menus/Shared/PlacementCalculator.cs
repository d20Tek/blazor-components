namespace D20Tek.BlazorComponents;

public static class PlacementCalculator
{
    public static MenuPosition Calculate(
        MenuRect trigger,
        MenuRect menu,
        MenuRect boundary,
        MenuPlacement preferred,
        double offset,
        bool flipEnabled = true,
        bool shiftEnabled = true)
    {
        var side = GetSide(preferred);
        var alignment = GetAlignment(preferred);

        if (flipEnabled)
        {
            side = ResolveSide(side, trigger, menu, boundary, offset);
        }

        var (x, y) = ComputeCoordinates(side, alignment, trigger, menu, offset);

        if (shiftEnabled)
        {
            (x, y) = Shift(side, x, y, menu, boundary);
        }

        return new MenuPosition(x, y, Compose(side, alignment));
    }

    private static MenuSide ResolveSide(MenuSide side, MenuRect trigger, MenuRect menu, MenuRect boundary, double offset)
    {
        if (FitsOnSide(side, trigger, menu, boundary, offset)) return side;

        var opposite = Opposite(side);
        return FitsOnSide(opposite, trigger, menu, boundary, offset) ? opposite : side;
    }

    private static bool FitsOnSide(MenuSide side, MenuRect trigger, MenuRect menu, MenuRect boundary, double offset) =>
        side switch
        {
            MenuSide.Bottom => trigger.Bottom + offset + menu.Height <= boundary.Bottom,
            MenuSide.Top => trigger.Top - offset - menu.Height >= boundary.Top,
            MenuSide.Left => trigger.Left - offset - menu.Width >= boundary.Left,
            MenuSide.Right => trigger.Right + offset + menu.Width <= boundary.Right,
            _ => true
        };

    private static (double X, double Y) ComputeCoordinates(
        MenuSide side, MenuAlignment alignment, MenuRect trigger, MenuRect menu, double offset)
    {
        double x;
        double y;

        switch (side)
        {
            case MenuSide.Bottom:
                y = trigger.Bottom + offset;
                x = AlignHorizontal(alignment, trigger, menu);
                break;
            case MenuSide.Top:
                y = trigger.Top - offset - menu.Height;
                x = AlignHorizontal(alignment, trigger, menu);
                break;
            case MenuSide.Left:
                x = trigger.Left - offset - menu.Width;
                y = AlignVertical(alignment, trigger, menu);
                break;
            default: // Right
                x = trigger.Right + offset;
                y = AlignVertical(alignment, trigger, menu);
                break;
        }

        return (x, y);
    }

    private static double AlignHorizontal(MenuAlignment alignment, MenuRect trigger, MenuRect menu) =>
        alignment == MenuAlignment.Start ? trigger.Left : trigger.Right - menu.Width;

    private static double AlignVertical(MenuAlignment alignment, MenuRect trigger, MenuRect menu) =>
        alignment == MenuAlignment.Start ? trigger.Top : trigger.Bottom - menu.Height;

    private static (double X, double Y) Shift(MenuSide side, double x, double y, MenuRect menu, MenuRect boundary)
    {
        // For top/bottom sides shift along X; for left/right sides shift along Y.
        if (side is MenuSide.Bottom or MenuSide.Top)
        {
            x = Clamp(x, boundary.Left, boundary.Right - menu.Width);
        }
        else
        {
            y = Clamp(y, boundary.Top, boundary.Bottom - menu.Height);
        }

        return (x, y);
    }

    private static double Clamp(double value, double min, double max)
    {
        if (max < min) return min;

        return value < min ? min : (value > max ? max : value);
    }

    private static MenuSide Opposite(MenuSide side) =>
        side switch
        {
            MenuSide.Bottom => MenuSide.Top,
            MenuSide.Top => MenuSide.Bottom,
            MenuSide.Left => MenuSide.Right,
            _ => MenuSide.Left
        };

    private static MenuSide GetSide(MenuPlacement placement) =>
        placement switch
        {
            MenuPlacement.BottomStart or MenuPlacement.BottomEnd => MenuSide.Bottom,
            MenuPlacement.TopStart or MenuPlacement.TopEnd => MenuSide.Top,
            MenuPlacement.LeftStart => MenuSide.Left,
            MenuPlacement.RightStart => MenuSide.Right,
            _ => MenuSide.Bottom
        };

    private static MenuAlignment GetAlignment(MenuPlacement placement) =>
        placement switch
        {
            MenuPlacement.BottomEnd or MenuPlacement.TopEnd => MenuAlignment.End,
            _ => MenuAlignment.Start
        };

    private static MenuPlacement Compose(MenuSide side, MenuAlignment alignment) =>
        (side, alignment) switch
        {
            (MenuSide.Bottom, MenuAlignment.Start) => MenuPlacement.BottomStart,
            (MenuSide.Bottom, MenuAlignment.End) => MenuPlacement.BottomEnd,
            (MenuSide.Top, MenuAlignment.Start) => MenuPlacement.TopStart,
            (MenuSide.Top, MenuAlignment.End) => MenuPlacement.TopEnd,
            (MenuSide.Left, _) => MenuPlacement.LeftStart,
            _ => MenuPlacement.RightStart
        };

    private enum MenuSide
    {
        Top,
        Bottom,
        Left,
        Right
    }

    private enum MenuAlignment
    {
        Start,
        End
    }
}
