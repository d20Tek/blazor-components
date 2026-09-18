namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class LinkTileTests
{
    [TestMethod]
    public void Render_WithHref_RendersAnchorRoot()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, "https://example.com"));

        // Assert
        var anchor = comp.Find("a");
        Assert.AreEqual("https://example.com", anchor.GetAttribute("href"));
        Assert.IsTrue(anchor.ClassList.Contains("tile"));
    }

    [TestMethod]
    public void Render_WithEmptyHref_FallsBackToButtonRoot()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, string.Empty));

        // Assert
        Assert.AreEqual(1, comp.FindAll("button").Count);
        Assert.AreEqual(0, comp.FindAll("a").Count);
    }

    [TestMethod]
    public void Render_WithTargetBlank_AddsSafeRel()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, "https://example.com")
            .Add(t => t.Target, "_blank"));

        // Assert
        var anchor = comp.Find("a");
        Assert.AreEqual("_blank", anchor.GetAttribute("target"));
        Assert.AreEqual("noopener noreferrer", anchor.GetAttribute("rel"));
    }

    [TestMethod]
    public void Render_WithTargetSelf_DoesNotAddRel()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, "https://example.com")
            .Add(t => t.Target, "_self"));

        // Assert
        Assert.IsFalse(comp.Find("a").HasAttribute("rel"));
    }

    [TestMethod]
    public void Render_WithConsumerRel_DoesNotOverrideRel()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, "https://example.com")
            .Add(t => t.Target, "_blank")
            .AddUnmatched("rel", "author"));

        // Assert
        Assert.AreEqual("author", comp.Find("a").GetAttribute("rel"));
    }

    [TestMethod]
    public void Click_WithClickedHandler_InvokesCallback()
    {
        // Arrange
        var ctx = new BunitContext();
        var clicked = false;
        var comp = ctx.Render<LinkTile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Href, "https://example.com")
            .Add(t => t.Clicked, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true)));

        // Act
        comp.Find("a").Click();

        // Assert
        Assert.IsTrue(clicked);
    }
}
