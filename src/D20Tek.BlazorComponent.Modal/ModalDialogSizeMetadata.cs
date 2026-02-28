namespace D20Tek.BlazorComponents;

internal class ModalDialogSizeMetadata
{
    private static readonly Dictionary<Size, string> _elements = new()
    {
        { Size.None, string.Empty },
        { Size.ExtraSmall, "modal-dialog-xs" },
        { Size.Small, "modal-dialog-sm" },
        { Size.Medium, "modal-dialog-md" },
        { Size.Large, "modal-dialog-lg" },
        { Size.ExtraLarge, "modal-dialog-xl" },
    };

    public static string GetSizeCss(Size size) => _elements[size];
}
