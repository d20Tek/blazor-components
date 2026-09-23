namespace D20Tek.BlazorComponents;

internal sealed class MenuInterop(IJSRuntime jsRuntime, FlyoutMenu owner)
{
    private IJSObjectReference? _module;
    private DotNetObjectReference<FlyoutMenu>? _dotNetRef;

    public async Task InitializeAsync(ElementReference trigger, ElementReference popup, object options)
    {
        try
        {
            _module ??= await jsRuntime.InvokeAsync<IJSObjectReference>(
                Constants.JSFunctions.Import, Constants.JSFunctions.ModulePath);
            _dotNetRef ??= DotNetObjectReference.Create(owner);

            await _module.InvokeVoidAsync(Constants.JSFunctions.Initialize, trigger, popup, _dotNetRef, options);
        }
        catch (Exception ex) when (IsBenign(ex))
        {
            // Interop unavailable (prerender/SSR/navigation); menu remains usable without positioning.
        }
    }

    public Task MoveFocusAsync(ElementReference popup, string direction) =>
        InvokeSafelyAsync(Constants.JSFunctions.MoveFocus, popup, direction);

    public Task TypeAheadAsync(ElementReference popup, string character) =>
        InvokeSafelyAsync(Constants.JSFunctions.TypeAhead, popup, character);

    public Task TeardownAsync(ElementReference popup) =>
        InvokeSafelyAsync(Constants.JSFunctions.Teardown, popup);

    public async Task FocusElementAsync(ElementReference element)
    {
        try
        {
            await element.FocusAsync();
        }
        catch (Exception ex) when (IsBenign(ex))
        {
        }
    }

    public async ValueTask DisposeAsync(ElementReference popup)
    {
        if (_module is not null)
        {
            try
            {
                await _module.InvokeVoidAsync(Constants.JSFunctions.Dispose, popup);
                await _module.DisposeAsync();
            }
            catch (Exception ex) when (ex is JSDisconnectedException or ObjectDisposedException)
            {
            }

            _module = null;
        }

        _dotNetRef?.Dispose();
        _dotNetRef = null;
    }

    private async Task InvokeSafelyAsync(string identifier, params object[] args)
    {
        if (_module is null) return;

        try
        {
            await _module.InvokeVoidAsync(identifier, args);
        }
        catch (Exception ex) when (IsBenign(ex))
        {
        }
    }

    internal static bool IsBenign(Exception ex) =>
        ex is JSDisconnectedException or ObjectDisposedException or InvalidOperationException;
}
