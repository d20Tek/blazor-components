namespace D20Tek.BlazorComponents;

public partial class ModalDialog : ModalDialogBase
{
    [Parameter]
    public bool ShowSubmitButton { get; set; } = true;

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Parameter]
    public EventCallback OnSubmit { get; set; }

    private async Task HandleClose()
    {
        await CloseAsync();
        await OnClose.InvokeAsync();
    }

    private async Task HandleSubmit()
    {
        await CloseAsync();
        await OnSubmit.InvokeAsync();
    }
}
