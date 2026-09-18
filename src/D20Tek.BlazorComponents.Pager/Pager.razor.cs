using System.Diagnostics.CodeAnalysis;

namespace D20Tek.BlazorComponents;

public partial class Pager : BaseComponent
{
    private static readonly ValueRange _currentPageRange = new(1, null);
    private static readonly ValueRange _pageSizeRange = new(1, null);
    private static readonly ValueRange _totalItemsRange = new(0, null);
    private static readonly ValueRange _boundaryCountRange = new(0, null);
    private static readonly ValueRange _middleCountRange = new(1, null);

    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalItems;
    private int _boundaryCount = 1;
    private int _middleCount = 5;

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Range validation required.")]
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPageRange.AssertInRange(value, nameof(CurrentPage));
            _currentPage = value;
        }
    }

    [Parameter]
    public EventCallback<int> CurrentPageChanged { get; set; }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Range validation required.")]
    public int PageSize
    {
        get => _pageSize;
        set
        {
            _pageSizeRange.AssertInRange(value, nameof(PageSize));
            _pageSize = value;
        }
    }

    [Parameter]
    public EventCallback<int> PageSizeChanged { get; set; }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Range validation required.")]
    public int TotalItems
    {
        get => _totalItems;
        set
        {
            _totalItemsRange.AssertInRange(value, nameof(TotalItems));
            _totalItems = value;
        }
    }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Range validation required.")]
    public int BoundaryCount
    {
        get => _boundaryCount;
        set
        {
            _boundaryCountRange.AssertInRange(value, nameof(BoundaryCount));
            _boundaryCount = value;
        }
    }

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "Range validation required.")]
    public int MiddleCount
    {
        get => _middleCount;
        set
        {
            _middleCountRange.AssertInRange(value, nameof(MiddleCount));
            _middleCount = value;
        }
    }

    [Parameter]
    public bool ShowNumbers { get; set; } = true;

    [Parameter]
    public bool ShowFirstLast { get; set; }

    [Parameter]
    public bool ShowDescription { get; set; }

    [Parameter]
    public bool ShowPageSizeSelector { get; set; }

    [Parameter]
    public IReadOnlyList<int> PageSizeOptions { get; set; } = [10, 20, 50, 100];

    [Parameter]
    public bool DisableResponsive { get; set; }

    [Parameter]
    public string PreviousLabel { get; set; } = "Previous";

    [Parameter]
    public string NextLabel { get; set; } = "Next";

    [Parameter]
    public string FirstLabel { get; set; } = "First";

    [Parameter]
    public string LastLabel { get; set; } = "Last";

    [Parameter]
    public string PageSizeLabel { get; set; } = "Rows:";

    internal int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalItems / (double)PageSize) : 0;

    internal PageWindow Window => PageWindow.Create(CurrentPage, TotalPages, BoundaryCount, MiddleCount);

    private async Task GoToPageAsync(int page)
    {
        var target = Math.Clamp(page, 1, Math.Max(1, TotalPages));
        if (target == CurrentPage || TotalPages <= 0) return;

        _currentPage = target;
        await CurrentPageChanged.InvokeAsync(target);
    }

    private async Task OnPageSizeChangedAsync(ChangeEventArgs args)
    {
        if (!int.TryParse(args.Value?.ToString(), out var newSize) || newSize < 1) return;
        if (newSize == PageSize) return;

        _pageSize = newSize;
        await PageSizeChanged.InvokeAsync(newSize);
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("pager")
            .AddClass(PagerSizeMetadata.GetSizeCss(Size), Size != Size.None)
            .AddClass("pager-static", DisableResponsive)
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
