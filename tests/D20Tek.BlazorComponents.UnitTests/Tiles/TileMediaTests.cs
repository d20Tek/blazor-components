namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileMediaTests
{
    [TestMethod]
    public void Render_WithImageUrl_RendersImageAndNoAvatar()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.ImageUrl, "https://example.com/a.png"));

        // Assert
        var img = comp.Find("img.tile-image");
        Assert.AreEqual("https://example.com/a.png", img.GetAttribute("src"));
        Assert.AreEqual("Hello", img.GetAttribute("alt"));
        Assert.IsEmpty(comp.FindAll(".tile-avatar"));
    }

    [TestMethod]
    public void Render_WithoutImageUrl_RendersAvatarWithInitials()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Mountain Retreat"));

        // Assert
        var avatar = comp.Find(".tile-avatar");
        Assert.AreEqual("MR", avatar.TextContent.Trim());
        Assert.IsEmpty(comp.FindAll("img.tile-image"));
    }

    [TestMethod]
    public void Render_WithIconCssClass_RendersIconAndNoImageOrAvatar()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.IconCssClass, "oi oi-transfer"));

        // Assert
        var icon = comp.Find("span.tile-icon");
        Assert.Contains("oi", icon.ClassList);
        Assert.Contains("oi-transfer", icon.ClassList);
        Assert.IsEmpty(comp.FindAll("img.tile-image"));
        Assert.IsEmpty(comp.FindAll(".tile-avatar"));
    }

    [TestMethod]
    public void Render_WithIconCssClassAndImageUrl_ImageTakesPrecedence()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.IconCssClass, "oi oi-transfer")
            .Add(t => t.ImageUrl, "https://example.com/a.png"));

        // Assert
        Assert.IsNotNull(comp.Find("img.tile-image"));
        Assert.IsEmpty(comp.FindAll("span.tile-icon"));
    }

    [TestMethod]
    public void Render_WithAbbreviationColor_AppliesBackgroundColor()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.AbbreviationColor, "#123456"));

        // Assert
        var style = comp.Find(".tile-avatar").GetAttribute("style");
        Assert.Contains("background-color: #123456", style!);
    }
}
