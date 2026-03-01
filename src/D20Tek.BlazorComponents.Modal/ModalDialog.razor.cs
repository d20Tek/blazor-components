namespace D20Tek.BlazorComponents;

public partial class ModalDialog : BaseComponent, IAsyncDisposable
{
    private const string _cssModalDialog = "modal-dialog";
    private ElementReference _dialogRef;
    private IJSObjectReference? _jsModule;

    public ModalDialog() => Size = Size.Medium;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Summary { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    [Parameter]
    public string SubmitButtonText { get; set; } = "Submit";

    [Parameter]
    public string CancelButtonText { get; set; } = "Cancel";

    [Parameter]
    public bool ShowSubmitButton { get; set; } = true;

    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    [Parameter]
    public EventCallback OnClose { get; set; }

    [Parameter]
    public EventCallback OnSubmit { get; set; }

    [Parameter]
    public VerticalPosition Position { get; set; } = VerticalPosition.Center;

    public bool IsOpen { get; private set; }

    protected override string? CalculateCssClasses() =>
        new CssBuilder(_cssModalDialog)
            .AddClass(ModalDialogSizeMetadata.GetSizeCss(Size), Size != Size.None)
            .AddClass(ModalDialogPositionMetadata.GetPositionCss(Position))
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder().AddStyleFromAttributes(RemainingAttributes)
                          .Build();

    public async Task ShowAsync()
    {
        await (await EnsureJsModuleAsync()).InvokeVoidAsync("showModal", _dialogRef);
        IsOpen = true;
        await InvokeAsync(StateHasChanged);
    }

    public async Task CloseAsync()
    {
        IsOpen = false;
        await (await EnsureJsModuleAsync()).InvokeVoidAsync("closeModal", _dialogRef);
        await InvokeAsync(StateHasChanged);
    }

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

    private async Task<IJSObjectReference> EnsureJsModuleAsync()
    {
        return _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/D20Tek.BlazorComponents.Modal/Modal.js");
    }

    public async ValueTask DisposeAsync()
    {
        if (_jsModule is not null)
        {
            await _jsModule.DisposeAsync();
            _jsModule = null;
        }
        GC.SuppressFinalize(this);
    }
}
