namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerRenderStateTests : BunitContext
{
    [TestMethod]
    public void Render_CurrentPage_MarksActivePageWithAriaCurrent()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 3)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100));

        // assert
        Assert.Contains("pager__page--active", cut.Markup);
        Assert.Contains("aria-current=\"page\"", cut.Markup);
    }

    [TestMethod]
    public void Render_FirstPage_DisablesPrevAndFirst()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowFirstLast, true));

        // assert
        var prev = cut.Find(".pager__prev");
        var first = cut.Find(".pager__first");
        Assert.IsTrue(prev.HasAttribute("disabled"));
        Assert.IsTrue(first.HasAttribute("disabled"));
    }

    [TestMethod]
    public void Render_LastPage_DisablesNextAndLast()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 10)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowFirstLast, true));

        // assert
        var next = cut.Find(".pager__next");
        var last = cut.Find(".pager__last");
        Assert.IsTrue(next.HasAttribute("disabled"));
        Assert.IsTrue(last.HasAttribute("disabled"));
    }

    [TestMethod]
    public void Render_LargePageCount_RendersEllipsis()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 10)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 200));

        // assert
        Assert.Contains("pager__ellipsis", cut.Markup);
    }
}
