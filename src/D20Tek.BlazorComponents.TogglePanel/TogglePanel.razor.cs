namespace D20Tek.BlazorComponents;

public partial class TogglePanel : BaseComponent
{
    private bool _isOpen = false;
    private bool _prevParentIsOpen = false;

    public TogglePanel() => Size = Size.Medium;

    [Parameter]
    public string Summary { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? SummaryContent { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool IsOpen { get; set; } = false;

    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

    [Parameter]
    public bool ShowChevron { get; set; } = true;

    protected override void OnParametersSet()
    {
        if (IsOpen != _prevParentIsOpen)
        {
            _isOpen = IsOpen;
            _prevParentIsOpen = IsOpen;
        }
        base.OnParametersSet();
    }

    private async Task HandleToggle()
    {
        _isOpen = !_isOpen;
        await IsOpenChanged.InvokeAsync(_isOpen);
        await OnToggle.InvokeAsync(_isOpen);
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("toggle-panel")
            .AddClass(TogglePanelSizeMetadata.GetSizeCss(Size))
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
