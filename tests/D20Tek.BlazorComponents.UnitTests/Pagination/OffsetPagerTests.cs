using D20Tek.Vertically.Queries.Pagination;

namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class OffsetPagerTests : BunitContext
{
    private static PageOf<string> CreatePage(int pageNumber, int pageSize, long totalCount)
    {
        var request = new PagedRequest { PageNumber = pageNumber, PageSize = pageSize };
        var items = Enumerable.Range(1, pageSize).Select(i => $"item-{i}").ToList();
        return PageOf<string>.Create(items, request, totalCount);
    }

    [TestMethod]
    public void Render_NullPage_RendersPagerWithSinglePage()
    {
        // arrange - act
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, (PageOf<string>?)null)
            .Add(p => p.ShowDescription, true));

        // assert
        Assert.Contains("pager", cut.Markup);
        Assert.Contains("Page 1 of 1", cut.Markup);
    }

    [TestMethod]
    public void Render_PopulatedPage_DrivesPagerFromMetadata()
    {
        // arrange - act
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(pageNumber: 2, pageSize: 10, totalCount: 55))
            .Add(p => p.ShowDescription, true));

        // assert
        Assert.Contains("Page 2 of 6", cut.Markup);
    }

    [TestMethod]
    public void Render_IsVisibleFalse_RendersNothing()
    {
        // arrange - act
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(1, 10, 50))
            .Add(p => p.IsVisible, false));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_PassesThroughToggles()
    {
        // arrange - act
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(3, 10, 100))
            .Add(p => p.ShowFirstLast, true)
            .Add(p => p.ShowPageSizeSelector, true));

        // assert
        Assert.Contains("pager__first", cut.Markup);
        Assert.Contains("pager__last", cut.Markup);
        Assert.Contains("pager__page-size", cut.Markup);
    }

    [TestMethod]
    public void ClickNext_RaisesOnPageQueryWithNextPageAndSamePageSize()
    {
        // arrange
        PagedRequest? captured = null;
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(pageNumber: 2, pageSize: 10, totalCount: 100))
            .Add(p => p.OnPageQuery, EventCallback.Factory.Create<PagedRequest>(this, r => captured = r)));

        // act
        cut.Find(".pager__next").Click();

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(3, captured.PageNumber);
        Assert.AreEqual(10, captured.PageSize);
    }

    [TestMethod]
    public void ClickPageNumber_RaisesOnPageQueryWithSelectedPage()
    {
        // arrange
        PagedRequest? captured = null;
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(pageNumber: 1, pageSize: 10, totalCount: 100))
            .Add(p => p.OnPageQuery, EventCallback.Factory.Create<PagedRequest>(this, r => captured = r)));

        // act
        cut.FindAll(".pager__page")[2].Click();

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(3, captured.PageNumber);
    }

    [TestMethod]
    public void ChangePageSize_RaisesOnPageQueryResetToFirstPage()
    {
        // arrange
        PagedRequest? captured = null;
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, CreatePage(pageNumber: 3, pageSize: 10, totalCount: 100))
            .Add(p => p.ShowPageSizeSelector, true)
            .Add(p => p.PageSizeOptions, new[] { 10, 25 })
            .Add(p => p.OnPageQuery, EventCallback.Factory.Create<PagedRequest>(this, r => captured = r)));

        // act
        cut.Find(".pager__page-size-select").Change("25");

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(1, captured.PageNumber);
        Assert.AreEqual(25, captured.PageSize);
    }

    [TestMethod]
    public void Render_EmptyPage_ShowsSinglePage()
    {
        // arrange
        var empty = PageOf<string>.Empty(new PagedRequest { PageNumber = 1, PageSize = 10 });

        // act
        var cut = Render<OffsetPager<string>>(parameters => parameters
            .Add(p => p.Page, empty)
            .Add(p => p.ShowDescription, true));

        // assert
        Assert.Contains("Page 1 of 1", cut.Markup);
    }
}
