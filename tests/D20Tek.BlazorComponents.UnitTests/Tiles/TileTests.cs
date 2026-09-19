namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileTests
{
    [TestMethod]
    public void Render_Default_RendersButtonRootWithTitle()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Hello"));

        // Assert
        var button = comp.Find("button");
        Assert.AreEqual("button", button.GetAttribute("type"));
        Assert.IsTrue(button.ClassList.Contains("tile"));
        Assert.AreEqual("Hello", comp.Find(".tile-title").TextContent.Trim());
    }

    [TestMethod]
    public void Render_DefaultSize_AppliesMediumModifier()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Hello"));

        // Assert
        Assert.IsTrue(comp.Find("button").ClassList.Contains("tile-md"));
    }

    [TestMethod]
    [DataRow(Size.ExtraSmall, "tile-xs")]
    [DataRow(Size.Large, "tile-lg")]
    public void Render_WithSize_AppliesSizeModifier(Size size, string expectedClass)
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Size, size));

        // Assert
        Assert.IsTrue(comp.Find("button").ClassList.Contains(expectedClass));
    }

    [TestMethod]
    [DataRow(LayoutOption.Compact, "tile-layout-compact")]
    [DataRow(LayoutOption.Verbose, "tile-layout-verbose")]
    public void Render_WithLayout_AppliesLayoutModifier(LayoutOption layout, string expectedClass)
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Layout, layout));

        // Assert
        Assert.IsTrue(comp.Find("button").ClassList.Contains(expectedClass));
    }

    [TestMethod]
    public void Render_IsVisibleFalse_RendersNothing()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.IsVisible, false));

        // Assert
        comp.MarkupMatches(string.Empty);
    }

    [TestMethod]
    public void Render_WithoutDescription_OmitsDescriptionElement()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Hello"));

        // Assert
        Assert.IsEmpty(comp.FindAll(".tile-description"));
    }

    [TestMethod]
    public void Render_WithDescription_RendersDescriptionElement()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Description, "A description."));

        // Assert
        Assert.AreEqual("A description.", comp.Find(".tile-description").TextContent.Trim());
    }

    [TestMethod]
    public void Render_WithoutFooter_OmitsFooterElement()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Hello"));

        // Assert
        Assert.IsEmpty(comp.FindAll(".tile-footer"));
    }

    [TestMethod]
    public void Render_WithFooter_RendersFooterContent()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Footer, (RenderFragment)(b => b.AddMarkupContent(0, "<span>foot</span>"))));

        // Assert
        Assert.Contains("foot", comp.Find(".tile-footer").InnerHtml);
    }

    [TestMethod]
    public void Render_UsesTitleAsAriaLabel()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Hello"));

        // Assert
        Assert.AreEqual("Hello", comp.Find("button").GetAttribute("aria-label"));
    }
}
