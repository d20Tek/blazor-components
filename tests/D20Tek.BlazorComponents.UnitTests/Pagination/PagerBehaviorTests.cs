namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerBehaviorTests : BunitContext
{
    [TestMethod]
    public void ClickNext_FromFirstPage_RaisesCurrentPageChangedWithNextPage()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        cut.Find(".pager__next").Click();

        // assert
        Assert.AreEqual(2, raised);
    }

    [TestMethod]
    public void ClickPrev_FromMiddlePage_RaisesCurrentPageChangedWithPreviousPage()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 3)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        cut.Find(".pager__prev").Click();

        // assert
        Assert.AreEqual(2, raised);
    }

    [TestMethod]
    public void ClickFirst_RaisesCurrentPageChangedWithOne()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 7)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowFirstLast, true)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        cut.Find(".pager__first").Click();

        // assert
        Assert.AreEqual(1, raised);
    }

    [TestMethod]
    public void ClickLast_RaisesCurrentPageChangedWithTotalPages()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowFirstLast, true)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        cut.Find(".pager__last").Click();

        // assert
        Assert.AreEqual(10, raised);
    }

    [TestMethod]
    public void ClickPageNumber_RaisesCurrentPageChangedWithThatPage()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        var thirdPage = cut.FindAll(".pager__page")[2];
        thirdPage.Click();

        // assert
        Assert.AreEqual(3, raised);
    }

    [TestMethod]
    public void ClickCurrentPage_DoesNotRaiseCurrentPageChanged()
    {
        // arrange
        var raised = false;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, [ExcludeFromCodeCoverage] (_) => raised = true)));

        // act
        var activePage = cut.Find(".pager__page--active");
        activePage.Click();

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public void ClickNext_OnLastPage_DoesNotRaiseCurrentPageChanged()
    {
        // arrange
        var raised = false;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 10)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.CurrentPageChanged, EventCallback.Factory.Create<int>(this, [ExcludeFromCodeCoverage] (_) => raised = true)));

        // act
        cut.Find(".pager__next").Click();

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public void ChangePageSize_RaisesPageSizeChangedWithNewValue()
    {
        // arrange
        var raised = 0;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 20)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowPageSizeSelector, true)
            .Add(p => p.PageSizeOptions, [20, 50])
            .Add(p => p.PageSizeChanged, EventCallback.Factory.Create<int>(this, v => raised = v)));

        // act
        cut.Find(".pager__page-size-select").Change("50");

        // assert
        Assert.AreEqual(50, raised);
    }

    [TestMethod]
    public void ChangePageSize_ToSameValue_DoesNotRaisePageSizeChanged()
    {
        // arrange
        var raised = false;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 20)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowPageSizeSelector, true)
            .Add(p => p.PageSizeOptions, [20, 50])
            .Add(p => p.PageSizeChanged, EventCallback.Factory.Create<int>(this, [ExcludeFromCodeCoverage] (_) => raised = true)));

        // act
        cut.Find(".pager__page-size-select").Change("20");

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public void ChangePageSize_ToInvalidValue_DoesNotRaisePageSizeChanged()
    {
        // arrange
        var raised = false;
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 20)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowPageSizeSelector, true)
            .Add(p => p.PageSizeOptions, [20, 50])
            .Add(p => p.PageSizeChanged, EventCallback.Factory.Create<int>(this, [ExcludeFromCodeCoverage] (_) => raised = true)));

        // act
        cut.Find(".pager__page-size-select").Change("abc");

        // assert
        Assert.IsFalse(raised);
    }
}
