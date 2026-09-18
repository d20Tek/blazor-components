namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerTotalPagesTests : BunitContext
{
    private int RenderAndGetTotalPages(int totalItems, int pageSize)
    {
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.TotalItems, totalItems)
            .Add(p => p.PageSize, pageSize));

        return cut.Instance.TotalPages;
    }

    [TestMethod]
    public void TotalPages_ExactDivision_ReturnsQuotient()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 100, pageSize: 10);

        // assert
        Assert.AreEqual(10, result);
    }

    [TestMethod]
    public void TotalPages_RemainderItems_RoundsUp()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 95, pageSize: 10);

        // assert
        Assert.AreEqual(10, result);
    }

    [TestMethod]
    public void TotalPages_SingleRemainderItem_RoundsUpToExtraPage()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 101, pageSize: 10);

        // assert
        Assert.AreEqual(11, result);
    }

    [TestMethod]
    public void TotalPages_ZeroItems_ReturnsZero()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 0, pageSize: 10);

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void TotalPages_FewerItemsThanPageSize_ReturnsSinglePage()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 3, pageSize: 10);

        // assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void TotalPages_ItemsEqualPageSize_ReturnsSinglePage()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 10, pageSize: 10);

        // assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void TotalPages_PageSizeOfOne_ReturnsTotalItems()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 7, pageSize: 1);

        // assert
        Assert.AreEqual(7, result);
    }

    [TestMethod]
    public void TotalPages_LargeItemCount_RoundsUp()
    {
        // arrange - act
        var result = RenderAndGetTotalPages(totalItems: 1_000_003, pageSize: 25);

        // assert
        Assert.AreEqual(40_001, result);
    }

    [TestMethod]
    public void TotalPages_ZeroPageSize_ReturnsZero()
    {
        // arrange
        var cut = Render<Pager>(parameters => parameters.Add(p => p.TotalItems, 100));
        PagerAccessor.SetPageSize(cut.Instance, 0);

        // act
        var result = cut.Instance.TotalPages;

        // assert
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void TotalPages_NegativePageSize_ReturnsZero()
    {
        // arrange
        var cut = Render<Pager>(parameters => parameters.Add(p => p.TotalItems, 100));
        PagerAccessor.SetPageSize(cut.Instance, -5);

        // act
        var result = cut.Instance.TotalPages;

        // assert
        Assert.AreEqual(0, result);
    }
}
