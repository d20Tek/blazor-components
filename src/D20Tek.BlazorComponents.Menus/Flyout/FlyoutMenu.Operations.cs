namespace D20Tek.BlazorComponents;

public partial class FlyoutMenu
{
    public async Task OpenAsync()
    {
        if (_isOpen || Disabled || !HasRenderableEntries()) return;

        _isOpen = true;
        UpdateContext();

        if (SingleOpen)
        {
            await MenuCoordinator.NotifyOpenedAsync(ResolvedGroup, this);
        }

        await OnOpen.InvokeAsync();
        _pendingInitialize = true;
        await SafeStateHasChangedAsync();
    }

    public async Task CloseAsync()
    {
        if (!_isOpen) return;

        _isOpen = false;
        UpdateContext();
        MenuCoordinator.NotifyClosed(ResolvedGroup, this);

        await _interop.TeardownAsync(_popupRef);
        await OnClose.InvokeAsync();
        await SafeStateHasChangedAsync();
    }

    public Task ToggleAsync() => _isOpen ? CloseAsync() : OpenAsync();

    private async Task HandleKeyDownAsync(KeyboardEventArgs e)
    {
        switch (MenuKeyboard.Resolve(e))
        {
            case MenuKeyCommand.CloseAndReturnFocus:
                await CloseAsync();
                await FocusTriggerAsync();
                break;
            case MenuKeyCommand.Close:
                await CloseAsync();
                break;
            case MenuKeyCommand.FocusNext:
                await _interop.MoveFocusAsync(_popupRef, "next");
                break;
            case MenuKeyCommand.FocusPrevious:
                await _interop.MoveFocusAsync(_popupRef, "previous");
                break;
            case MenuKeyCommand.FocusFirst:
                await _interop.MoveFocusAsync(_popupRef, "first");
                break;
            case MenuKeyCommand.FocusLast:
                await _interop.MoveFocusAsync(_popupRef, "last");
                break;
            case MenuKeyCommand.TypeAhead:
                await _interop.TypeAheadAsync(_popupRef, e.Key);
                break;
            default:
                break;
        }
    }
}
