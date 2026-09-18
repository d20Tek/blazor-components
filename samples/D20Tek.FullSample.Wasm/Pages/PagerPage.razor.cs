using D20Tek.BlazorComponents;
using D20Tek.Vertically.Queries.Pagination;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class PagerPage
{
    private static readonly string[] _catalog = Enumerable.Range(1, 240).Select(i => $"Catalog item #{i}").ToArray();

    private int _currentPage = 1;
    private int _pageSize = 10;
    private int _totalItems = 240;
    private int _boundaryCount = 1;
    private int _middleCount = 5;
    private Size _size = Size.Small;
    private bool _showFirstLast;
    private bool _showDescription = true;
    private bool _showPageSizeSelector;
    private bool _disableResponsive;

    private int _defaultPage = 1;
    private int _fullPage = 1;
    private int _fullPageSize = 20;
    private int _responsivePage = 1;

    private PageOf<string> _verticalPage = default!;

    protected override void OnInitialized() => _verticalPage = LoadPage(new PagedRequest { PageNumber = 1, PageSize = 5 });

    private void OnCurrentPageChanged(int page) => _currentPage = page;

    private void OnPageSizeChanged(int pageSize)
    {
        _pageSize = pageSize;
        _currentPage = 1;
    }

    private void OnPageQuery(PagedRequest request) => _verticalPage = LoadPage(request);

    private static PageOf<string> LoadPage(PagedRequest request)
    {
        var items = _catalog.Skip(request.Skip)
                            .Take(request.Take)
                            .ToList();

        return PageOf<string>.Create(items, request, _catalog.Length);
    }
}
