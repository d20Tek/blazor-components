namespace D20Tek.BlazorComponents;

public abstract class ModalDialogBase : BaseComponent, IAsyncDisposable
{
    private IJSObjectReference? _jsModule;

    protected ElementReference _dialogRef;

    public ModalDialogBase() => Size = Size.Medium;

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
    public string SubmitButtonText { get; set; } = Constants.SubmitButtonTextDefault;

    [Parameter]
    public string CancelButtonText { get; set; } = Constants.CancelButtonTextDefault;

    [Parameter]
    public bool ShowCancelButton { get; set; } = true;

    [Parameter]
    public VerticalPosition Position { get; set; } = VerticalPosition.Center;

    public bool IsOpen { get; private set; }

    protected override string? CalculateCssClasses() =>
        new CssBuilder(Constants.CssModalDialog)
            .AddClass(ModalDialogSizeMetadata.GetSizeCss(Size), Size != Size.None)
            .AddClass(ModalDialogPositionMetadata.GetPositionCss(Position))
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder().AddStyleFromAttributes(RemainingAttributes).Build();

    public async Task ShowAsync()
    {
        await (await EnsureJsModuleAsync()).InvokeVoidAsync(Constants.JSFunctions.ShowModal, _dialogRef);
        IsOpen = true;
        await InvokeAsync(StateHasChanged);
    }

    public async Task CloseAsync()
    {
        IsOpen = false;
        await (await EnsureJsModuleAsync()).InvokeVoidAsync(Constants.JSFunctions.CloseModal, _dialogRef);
        await InvokeAsync(StateHasChanged);
    }

    private async Task<IJSObjectReference> EnsureJsModuleAsync() =>
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
            Constants.JSFunctions.Import,
            Constants.JSFunctions.ModulePath);

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
