namespace D20Tek.BlazorComponents;

internal readonly struct PageWindow
{
    private PageWindow(
        int currentPage,
        int totalPages,
        IReadOnlyList<int> pages,
        bool hasLeadingEllipsis,
        bool hasTrailingEllipsis)
    {
        CurrentPage = currentPage;
        TotalPages = totalPages;
        Pages = pages;
        HasLeadingEllipsis = hasLeadingEllipsis;
        HasTrailingEllipsis = hasTrailingEllipsis;
    }

    public int CurrentPage { get; }

    public int TotalPages { get; }

    public IReadOnlyList<int> Pages { get; }

    public bool HasLeadingEllipsis { get; }

    public bool HasTrailingEllipsis { get; }

    public bool HasPreviousPage => CurrentPage > 1 && TotalPages > 0;

    public bool HasNextPage => CurrentPage < TotalPages;

    public bool HasFirstPage => HasPreviousPage;

    public bool HasLastPage => HasNextPage;

    public static PageWindow Create(int currentPage, int totalPages, int boundaryCount, int middleCount)
    {
        totalPages = Math.Max(0, totalPages);
        boundaryCount = Math.Max(0, boundaryCount);
        middleCount = Math.Max(1, middleCount);

        if (totalPages <= 0)
        {
            return new PageWindow(0, 0, [], false, false);
        }

        currentPage = Math.Clamp(currentPage, 1, totalPages);

        var half = (middleCount - 1) / 2;
        var middleStart = Math.Max(1, currentPage - half);
        var middleEnd = Math.Min(totalPages, middleStart + middleCount - 1);
        middleStart = Math.Max(1, middleEnd - middleCount + 1);

        var included = new SortedSet<int>();

        for (var i = 1; i <= boundaryCount && i <= totalPages; i++)
        {
            included.Add(i);
        }

        for (var i = totalPages - boundaryCount + 1; i <= totalPages; i++)
        {
            if (i >= 1) included.Add(i);
        }

        for (var i = middleStart; i <= middleEnd; i++)
        {
            included.Add(i);
        }

        var pages = included.ToList();
        var hasLeadingEllipsis = false;
        var hasTrailingEllipsis = false;

        for (var i = 0; i < pages.Count - 1; i++)
        {
            if (pages[i + 1] - pages[i] > 1)
            {
                if (pages[i + 1] <= middleStart)
                {
                    hasLeadingEllipsis = true;
                }
                else
                {
                    hasTrailingEllipsis = true;
                }
            }
        }

        return new PageWindow(currentPage, totalPages, pages, hasLeadingEllipsis, hasTrailingEllipsis);
    }
}
