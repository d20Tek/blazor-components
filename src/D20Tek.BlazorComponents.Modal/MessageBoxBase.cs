namespace D20Tek.BlazorComponents;

public abstract class MessageBoxBase : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? _jsModule;

    protected ElementReference _dialogRef;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public string Title { get; set; } = Constants.MessageTextDefault;

    protected string? CssClass => new CssBuilder(Constants.CssModalDialog).AddClass("modal-dialog-sm").Build();

    protected string? CssStyles => new StyleBuilder().Build();

    public async Task ShowAsync()
    {
        await (await EnsureJsModuleAsync()).InvokeVoidAsync(Constants.JSFunctions.ShowModal, _dialogRef);
        await InvokeAsync(StateHasChanged);
    }

    public async Task CloseAsync()
    {
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
