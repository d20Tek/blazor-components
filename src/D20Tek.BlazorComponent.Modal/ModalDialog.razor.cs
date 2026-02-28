namespace D20Tek.BlazorComponents;

public partial class ModalDialog : BaseComponent, IAsyncDisposable
{
    private const string _cssModalDialog = "modal-dialog";
    private readonly string _dialogId = $"modal-{Guid.NewGuid():N}";
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
        await EnsureJsModule();
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("showModal", _dialogId);
            IsOpen = true;
        }
    }

    public async Task CloseAsync()
    {
        await EnsureJsModule();
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("closeModal", _dialogId);
            IsOpen = false;
        }
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

    private async Task EnsureJsModule()
    {
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            "./_content/D20Tek.BlazorComponent.Modal/modalDialog.js");
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
