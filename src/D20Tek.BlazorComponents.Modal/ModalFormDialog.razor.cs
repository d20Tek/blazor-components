namespace D20Tek.BlazorComponents;

public partial class ModalFormDialog : ModalDialogBase
{
    [Parameter]
    [EditorRequired]
    public object Model { get; set; } = default!;

    [Parameter]
    public EventCallback<EditContext> OnValidSubmit { get; set; }

    [Parameter]
    public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private async Task HandleCancel()
    {
        await CloseAsync();
        await OnCancel.InvokeAsync();
    }

    private async Task HandleValidSubmit(EditContext context)
    {
        await OnValidSubmit.InvokeAsync(context);
    }

    private async Task HandleInvalidSubmit(EditContext context)
    {
        await OnInvalidSubmit.InvokeAsync(context);
    }
}
