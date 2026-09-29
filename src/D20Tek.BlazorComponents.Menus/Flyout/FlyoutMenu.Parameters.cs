namespace D20Tek.BlazorComponents;

public partial class FlyoutMenu
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public RenderFragment<FlyoutMenuContext>? Trigger { get; set; }

    [Parameter]
    public string? TriggerIcon { get; set; }

    [Parameter]
    public string TriggerAriaLabel { get; set; } = Constants.DefaultTriggerAriaLabel;

    [Parameter]
    public string? TriggerCssClass { get; set; }

    [Parameter]
    public IReadOnlyList<IMenuEntry>? Items { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public MenuPlacement Placement { get; set; } = MenuPlacement.BottomEnd;

    [Parameter]
    public int Offset { get; set; } = Constants.DefaultOffset;

    [Parameter]
    public string? Boundary { get; set; }

    [Parameter]
    public bool FlipEnabled { get; set; } = true;

    [Parameter]
    public bool ShiftEnabled { get; set; } = true;

    [Parameter]
    public bool SingleOpen { get; set; } = true;

    [Parameter]
    public string? GroupName { get; set; }

    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public bool CloseOnScroll { get; set; }

    [Parameter]
    public bool CloseOnOutsideClick { get; set; } = true;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public int? ZIndex { get; set; }

    [Parameter]
    public bool Animate { get; set; } = true;

    [Parameter]
    public string? MenuCssClass { get; set; }

    [Parameter]
    public EventCallback OnOpen { get; set; }

    [Parameter]
    public EventCallback OnClose { get; set; }

    public bool IsOpen => _isOpen;

    private string PopupId => string.IsNullOrEmpty(Id) ? _popupId : Id!;

    private string ResolvedGroup => string.IsNullOrEmpty(GroupName) ? Constants.DefaultGroupName : GroupName!;

    private string? TriggerButtonCss => MenuCss.TriggerButtonClasses(_isOpen, TriggerCssClass);

    private string? PopupCss => MenuCss.PopupClasses(Size, Animate, MenuCssClass);

    private string? PopupStyle => MenuCss.PopupStyle(ZIndex);
}
