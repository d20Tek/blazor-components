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

    public bool IsOpen { get; private set; }

    protected override string? CalculateCssClasses() =>
        new CssBuilder(_cssModalDialog).AddClass(ModalDialogSizeMetadata.GetSizeCss(Size), Size != Size.None)
                                       .AddClassFromAttributes(RemainingAttributes)
                                       .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder().AddStyleFromAttributes(RemainingAttributes)
                          .Build();

    public async Task ShowAsync()
    {
        await EnsureJsModuleAsync();
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("showModal", _dialogRef);
            IsOpen = true;
            await InvokeAsync(StateHasChanged);
        }
    }

    public async Task CloseAsync()
    {
        IsOpen = false;
        await EnsureJsModuleAsync();
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("closeModal", _dialogRef);
            await InvokeAsync(StateHasChanged);
        }
    }

    private async Task HandleClose()
    {
        IsOpen = false;
        try
        {
            await EnsureJsModuleAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("closeModal", _dialogRef);
            }
        }
        catch { /* Dialog might already be closed */ }
        await OnClose.InvokeAsync();
    }

    private async Task HandleSubmit()
    {
        IsOpen = false;
        try
        {
            await EnsureJsModuleAsync();
            if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("closeModal", _dialogRef);
            }
        }
        catch { /* Dialog might already be closed */ }
        await OnSubmit.InvokeAsync();
    }

    private async Task EnsureJsModuleAsync()
    {
        if (_jsModule is null)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/D20Tek.BlazorComponent.Modal/modalDialog.js");
        }
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
