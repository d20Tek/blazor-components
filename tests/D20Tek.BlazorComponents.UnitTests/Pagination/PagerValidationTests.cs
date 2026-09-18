namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerValidationTests : BunitContext
{
    [TestMethod]
    public void CurrentPage_BelowMinimum_ThrowsArgumentOutOfRange()
    {
        // arrange - act - assert
        [ExcludeFromCodeCoverage]
        void Act() => Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 0)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50));

        var ex = Assert.ThrowsExactly<InvalidOperationException>(Act);
        Assert.IsInstanceOfType<ArgumentOutOfRangeException>(ex.InnerException);
    }

    [TestMethod]
    public void PageSize_BelowMinimum_ThrowsArgumentOutOfRange()
    {
        // arrange - act - assert
        [ExcludeFromCodeCoverage]
        void Act() => Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 0)
            .Add(p => p.TotalItems, 50));

        var ex = Assert.ThrowsExactly<InvalidOperationException>(Act);
        Assert.IsInstanceOfType<ArgumentOutOfRangeException>(ex.InnerException);
    }

    [TestMethod]
    public void TotalItems_Negative_ThrowsArgumentOutOfRange()
    {
        // arrange - act - assert
        [ExcludeFromCodeCoverage]
        void Act() => Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, -1));

        var ex = Assert.ThrowsExactly<InvalidOperationException>(Act);
        Assert.IsInstanceOfType<ArgumentOutOfRangeException>(ex.InnerException);
    }

    [TestMethod]
    public void BoundaryCount_Negative_ThrowsArgumentOutOfRange()
    {
        // arrange - act - assert
        [ExcludeFromCodeCoverage]
        void Act() => Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.BoundaryCount, -1));

        var ex = Assert.ThrowsExactly<InvalidOperationException>(Act);
        Assert.IsInstanceOfType<ArgumentOutOfRangeException>(ex.InnerException);
    }

    [TestMethod]
    public void MiddleCount_BelowMinimum_ThrowsArgumentOutOfRange()
    {
        // arrange - act - assert
        [ExcludeFromCodeCoverage]
        void Act() => Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.MiddleCount, 0));

        var ex = Assert.ThrowsExactly<InvalidOperationException>(Act);
        Assert.IsInstanceOfType<ArgumentOutOfRangeException>(ex.InnerException);
    }

    [TestMethod]
    public void TotalItems_Zero_RendersSinglePageDescription()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 0)
            .Add(p => p.ShowDescription, true));

        // assert
        Assert.Contains("Page 1 of 1", cut.Markup);
    }
}
