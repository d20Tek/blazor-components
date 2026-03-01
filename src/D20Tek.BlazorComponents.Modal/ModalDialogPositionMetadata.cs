namespace D20Tek.BlazorComponents;

internal class ModalDialogPositionMetadata
{
    private static readonly Dictionary<VerticalPosition, string> _elements = new()
    {
        { VerticalPosition.Center, "modal-dialog--center" },
        { VerticalPosition.Top, "modal-dialog--top" },
        { VerticalPosition.Bottom, "modal-dialog--bottom" },
    };

    public static string GetPositionCss(VerticalPosition position) => _elements[position];
}
