namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerRenderStyleTests : BunitContext
{
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
