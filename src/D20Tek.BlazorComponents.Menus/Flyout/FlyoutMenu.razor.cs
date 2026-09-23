namespace D20Tek.BlazorComponents;

public partial class FlyoutMenu : BaseComponent, IManagedMenu, IAsyncDisposable
{
    private readonly string _popupId = $"d20tek-menu-{Guid.NewGuid():N}";
    private FlyoutMenuContext _context = default!;
    private MenuInterop _interop = default!;
    private ElementReference _triggerRef;
    private ElementReference _popupRef;
    private bool _isOpen;
    private bool _disposed;
    private bool _pendingInitialize;

    public FlyoutMenu()
    {
        Size = Size.Medium;
        _context = new FlyoutMenuContext(false, EventCallback.Factory.Create(this, ToggleAsync));
    }

    protected override string? CalculateCssClasses() => MenuCss.RootClasses(Size, _isOpen, RemainingAttributes);

    protected override string? CalculateCssStyles() => MenuCss.RootStyles(RemainingAttributes);

    protected override void OnInitialized() => _interop = new MenuInterop(JSRuntime, this);

    async Task IManagedMenu.RequestCloseAsync() => await CloseAsync();

    [JSInvokable]
    public async Task OnOutsideInteraction()
    {
        if (CloseOnOutsideClick) await CloseAsync();
    }

    [JSInvokable]
    public async Task OnScrollInteraction()
    {
        if (CloseOnScroll) await CloseAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_pendingInitialize && _isOpen)
        {
            _pendingInitialize = false;
            await _interop.InitializeAsync(_triggerRef, _popupRef, BuildInteropOptions());
        }
    }

    private object BuildInteropOptions() =>
        new
        {
            placement = (int)Placement,
            offset = Offset,
            boundary = Boundary,
            flip = FlipEnabled,
            shift = ShiftEnabled,
            closeOnScroll = CloseOnScroll,
            closeOnOutsideClick = CloseOnOutsideClick
        };

    private async Task FocusTriggerAsync()
    {
        try
        {
            await _triggerRef.FocusAsync();
        }
        catch (Exception ex) when (MenuInterop.IsBenign(ex))
        {
        }
    }

    private bool HasRenderableEntries()
    {
        if (Items is not null && Items.Any(e => e is not MenuSeparatorItem and not MenuHeaderItem)) return true;

        return ChildContent is not null;
    }

    private void UpdateContext() => _context = new FlyoutMenuContext(_isOpen, EventCallback.Factory.Create(this, ToggleAsync));

    private async Task SafeStateHasChangedAsync()
    {
        if (!_disposed) await InvokeAsync(StateHasChanged);
    }

    private RenderFragment RenderEntry(IMenuEntry entry) =>
        builder => MenuEntryRenderer.Render(builder, entry, this);

    internal EventCallback WrapClose(bool closeOnClick, EventCallback inner) =>
        EventCallback.Factory.Create(this, async () =>
        {
            await inner.InvokeAsync();
            if (closeOnClick)
            {
                await CloseAsync();
            }
        });

    public async ValueTask DisposeAsync()
    {
        _disposed = true;
        MenuCoordinator.NotifyClosed(ResolvedGroup, this);

        if (_interop is not null)
        {
            await _interop.DisposeAsync(_popupRef);
        }

        GC.SuppressFinalize(this);
    }
}
