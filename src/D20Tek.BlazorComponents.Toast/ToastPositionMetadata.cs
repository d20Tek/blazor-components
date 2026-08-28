namespace D20Tek.BlazorComponents;

internal class ToastPositionMetadata
{
    private static readonly Dictionary<ToastPosition, string> _elements = new()
    {
        { ToastPosition.TopLeft, "toast-host-top-left" },
        { ToastPosition.TopCenter, "toast-host-top-center" },
        { ToastPosition.TopRight, "toast-host-top-right" },
        { ToastPosition.MiddleLeft, "toast-host-middle-left" },
        { ToastPosition.MiddleCenter, "toast-host-middle-center" },
        { ToastPosition.MiddleRight, "toast-host-middle-right" },
        { ToastPosition.BottomLeft, "toast-host-bottom-left" },
        { ToastPosition.BottomCenter, "toast-host-bottom-center" },
        { ToastPosition.BottomRight, "toast-host-bottom-right" },
    };

    public static string GetPositionCss(ToastPosition position) => _elements[position];
}
