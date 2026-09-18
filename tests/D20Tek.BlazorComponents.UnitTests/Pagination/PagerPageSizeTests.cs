namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerPageSizeTests : BunitContext
{
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

    [TestMethod]
    public void ChangePageSize_ToEmptyValue_DoesNotRaisePageSizeChanged()
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
        cut.Find(".pager__page-size-select").Change(string.Empty);

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public void ChangePageSize_ToZero_DoesNotRaisePageSizeChanged()
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
        cut.Find(".pager__page-size-select").Change("0");

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public void ChangePageSize_ToNegativeValue_DoesNotRaisePageSizeChanged()
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
        cut.Find(".pager__page-size-select").Change("-5");

        // assert
        Assert.IsFalse(raised);
    }

    [TestMethod]
    public async Task ChangePageSize_WithNullEventValue_DoesNotRaisePageSizeChanged()
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
        await cut.InvokeAsync(async () =>
            await cut.Instance.OnPageSizeChangedAsync(new ChangeEventArgs { Value = null }));

        // assert
        Assert.IsFalse(raised);
    }
}
