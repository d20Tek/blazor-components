namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerRenderTests : BunitContext
{
    [TestMethod]
    public void Render_Defaults_ShowsNavPrevNextAndNumbers()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50));

        // assert
        Assert.Contains("role=\"navigation\"", cut.Markup);
        Assert.Contains("aria-label=\"pagination\"", cut.Markup);
        Assert.Contains("pager__prev", cut.Markup);
        Assert.Contains("pager__next", cut.Markup);
        Assert.Contains("pager__pages", cut.Markup);
    }

    [TestMethod]
    public void Render_Defaults_HidesOptInRegions()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50));

        // assert
        Assert.DoesNotContain("pager__first", cut.Markup);
        Assert.DoesNotContain("pager__last", cut.Markup);
        Assert.DoesNotContain("pager__description", cut.Markup);
        Assert.DoesNotContain("pager__page-size", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowFirstLast_RendersFirstAndLastButtons()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 3)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowFirstLast, true));

        // assert
        Assert.Contains("pager__first", cut.Markup);
        Assert.Contains("pager__last", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowDescription_RendersPageOfText()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 2)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 55)
            .Add(p => p.ShowDescription, true));

        // assert
        Assert.Contains("pager__description", cut.Markup);
        Assert.Contains("Page 2 of 6", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowPageSizeSelector_RendersSelectWithOptions()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 20)
            .Add(p => p.TotalItems, 100)
            .Add(p => p.ShowPageSizeSelector, true)
            .Add(p => p.PageSizeOptions, new[] { 20, 40 }));

        // assert
        Assert.Contains("pager__page-size-select", cut.Markup);
        Assert.Contains("<option value=\"20\"", cut.Markup);
        Assert.Contains("<option value=\"40\"", cut.Markup);
    }

    [TestMethod]
    public void Render_AllOptInRegionsOn_RendersEveryRegion()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 5)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 200)
            .Add(p => p.ShowFirstLast, true)
            .Add(p => p.ShowDescription, true)
            .Add(p => p.ShowPageSizeSelector, true));

        // assert
        Assert.Contains("pager__description", cut.Markup);
        Assert.Contains("pager__first", cut.Markup);
        Assert.Contains("pager__last", cut.Markup);
        Assert.Contains("pager__pages", cut.Markup);
        Assert.Contains("pager__page-size", cut.Markup);
        Assert.Contains("pager__compact", cut.Markup);
    }

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

    [TestMethod]
    public void Render_SizeMedium_AppliesSizeModifierClass()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.Size, Size.Medium));

        // assert
        Assert.Contains("pager-md", cut.Markup);
    }

    [TestMethod]
    public void Render_SizeNone_OmitsSizeModifierClass()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.Size, Size.None));

        // assert
        Assert.DoesNotContain("pager-sm", cut.Markup);
        Assert.DoesNotContain("pager-md", cut.Markup);
    }

    [TestMethod]
    public void Render_DisableResponsive_AddsStaticClass()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.DisableResponsive, true));

        // assert
        Assert.Contains("pager-static", cut.Markup);
    }

    [TestMethod]
    public void Render_ResponsiveDefault_OmitsStaticClass()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50));

        // assert
        Assert.DoesNotContain("pager-static", cut.Markup);
    }

    [TestMethod]
    public void Render_IsVisibleFalse_RendersNothing()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.IsVisible, false));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_ShowNumbersFalse_OmitsPageList()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .Add(p => p.ShowNumbers, false));

        // assert
        Assert.DoesNotContain("pager__pages", cut.Markup);
    }

    [TestMethod]
    public void Render_RemainingAttributes_ForwardsCustomClass()
    {
        // arrange - act
        var cut = Render<Pager>(parameters => parameters
            .Add(p => p.CurrentPage, 1)
            .Add(p => p.PageSize, 10)
            .Add(p => p.TotalItems, 50)
            .AddUnmatched("class", "custom-pager"));

        // assert
        Assert.Contains("custom-pager", cut.Markup);
    }
}
