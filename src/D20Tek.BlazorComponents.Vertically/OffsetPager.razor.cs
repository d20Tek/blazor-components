namespace D20Tek.BlazorComponents;

public partial class OffsetPager<T> : ComponentBase
{
    [Parameter]
    public PageOf<T>? Page { get; set; }

    [Parameter]
    public EventCallback<PagedRequest> OnPageQuery { get; set; }

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
    public int BoundaryCount { get; set; } = 1;

    [Parameter]
    public int MiddleCount { get; set; } = 5;

    [Parameter]
    public bool DisableResponsive { get; set; }

    [Parameter]
    public Size Size { get; set; } = Size.Small;

    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> RemainingAttributes { get; set; } = [];

    private int CurrentPage => Page?.PageNumber ?? 1;

    private int PageSize => Page?.PageSize ?? (PageSizeOptions.Count > 0 ? PageSizeOptions[0] : 10);

    private int TotalItems => Page is null ? 0 : (int)Math.Min(int.MaxValue, Page.TotalCount);

    private Task OnCurrentPageChangedAsync(int pageNumber) =>
        OnPageQuery.InvokeAsync(new PagedRequest { PageNumber = pageNumber, PageSize = PageSize });

    private Task OnPageSizeChangedAsync(int pageSize) =>
        OnPageQuery.InvokeAsync(new PagedRequest { PageNumber = 1, PageSize = pageSize });
}
