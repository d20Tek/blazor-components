namespace D20Tek.BlazorComponents;

public readonly record struct MenuRect(double X, double Y, double Width, double Height)
{
    public double Left => X;

    public double Top => Y;

    public double Right => X + Width;

    public double Bottom => Y + Height;
}
