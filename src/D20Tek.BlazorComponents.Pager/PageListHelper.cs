namespace D20Tek.BlazorComponents;

internal static class PageListHelper
{
    public static IReadOnlyList<int> BuildIncludedPages(
        int totalPages,
        int boundaryCount,
        int middleStart,
        int middleEnd)
    {
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

        return [.. included];
    }

    public static (bool HasLeadingEllipsis, bool HasTrailingEllipsis) CalculateEllipses(
        IReadOnlyList<int> pages,
        int middleStart)
    {
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

        return (hasLeadingEllipsis, hasTrailingEllipsis);
    }
}
