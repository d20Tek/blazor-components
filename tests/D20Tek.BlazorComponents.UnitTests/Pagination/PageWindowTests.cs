namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PageWindowTests
{
    [TestMethod]
    public void Create_ZeroTotalPages_ReturnsEmptyWindow()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 1, totalPages: 0, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.IsEmpty(window.Pages);
        Assert.AreEqual(0, window.TotalPages);
        Assert.IsFalse(window.HasPreviousPage);
        Assert.IsFalse(window.HasNextPage);
        Assert.IsFalse(window.HasFirstPage);
        Assert.IsFalse(window.HasLastPage);
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsFalse(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_SinglePage_ReturnsSinglePageNoEllipsis()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 1, totalPages: 1, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1], window.Pages.ToList());
        Assert.IsFalse(window.HasPreviousPage);
        Assert.IsFalse(window.HasNextPage);
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsFalse(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_FewPagesNoGap_RendersAllWithoutEllipsis()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 2, totalPages: 5, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 2, 3, 4, 5], window.Pages.ToList());
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsFalse(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_CurrentAtMiddle_CentersFivePagesWithBothEllipses()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 10, totalPages: 20, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 8, 9, 10, 11, 12, 20], window.Pages.ToList());
        Assert.IsTrue(window.HasLeadingEllipsis);
        Assert.IsTrue(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_CurrentAtStart_OnlyTrailingEllipsis()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 1, totalPages: 20, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 2, 3, 4, 5, 20], window.Pages.ToList());
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsTrue(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_CurrentAtEnd_OnlyLeadingEllipsis()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 20, totalPages: 20, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 16, 17, 18, 19, 20], window.Pages.ToList());
        Assert.IsTrue(window.HasLeadingEllipsis);
        Assert.IsFalse(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_MiddleOverlapsBoundary_DeduplicatesPages()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 3, totalPages: 20, boundaryCount: 2, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 2, 3, 4, 5, 19, 20], window.Pages.ToList());
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsTrue(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_MiddleCountLargerThanTotal_RendersAllPages()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 2, totalPages: 4, boundaryCount: 1, middleCount: 20);

        // assert
        Assert.AreSequenceEqual([1, 2, 3, 4], window.Pages.ToList());
        Assert.IsFalse(window.HasLeadingEllipsis);
        Assert.IsFalse(window.HasTrailingEllipsis);
    }

    [TestMethod]
    public void Create_BoundaryCountLargerThanTotal_RendersAllPagesOnce()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 2, totalPages: 4, boundaryCount: 10, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([1, 2, 3, 4], window.Pages.ToList());
    }

    [TestMethod]
    public void Create_ZeroBoundaryCount_OmitsAnchoredEnds()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 10, totalPages: 20, boundaryCount: 0, middleCount: 5);

        // assert
        Assert.AreSequenceEqual([8, 9, 10, 11, 12], window.Pages.ToList());
    }

    [TestMethod]
    public void Create_CurrentPageBelowRange_ClampsToOne()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: -5, totalPages: 10, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreEqual(1, window.CurrentPage);
        Assert.IsFalse(window.HasPreviousPage);
    }

    [TestMethod]
    public void Create_CurrentPageAboveRange_ClampsToTotal()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 99, totalPages: 10, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.AreEqual(10, window.CurrentPage);
        Assert.IsFalse(window.HasNextPage);
    }

    [TestMethod]
    public void Create_NegativeTotalPages_ReturnsEmptyWindow()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 1, totalPages: -3, boundaryCount: 1, middleCount: 5);

        // assert
        Assert.IsEmpty(window.Pages);
    }

    [TestMethod]
    public void Create_MiddleCountBelowOne_TreatedAsOne()
    {
        // arrange - act
        var window = PageWindow.Create(currentPage: 5, totalPages: 10, boundaryCount: 0, middleCount: 0);

        // assert
        Assert.AreSequenceEqual([5], window.Pages.ToList());
    }
}
