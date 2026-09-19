namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileBaseTests
{
    [TestMethod]
    public void ResolvedHref_OnNonLinkTile_ReturnsNull()
    {
        // Arrange
        var tile = new Tile();

        // Act
        var result = ((TileBase)tile).ResolvedHref;

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ResolvedTarget_OnNonLinkTile_ReturnsNull()
    {
        // Arrange
        var tile = new Tile();

        // Act
        var result = ((TileBase)tile).ResolvedTarget;

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void AriaLabel_WithTitle_ReturnsTitle()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Mountain Retreat"));

        // Assert
        Assert.AreEqual("Mountain Retreat", comp.Find("button").GetAttribute("aria-label"));
    }

    [TestMethod]
    public void AriaLabel_WithWhitespaceTitle_ReturnsInitials()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "   ")
            .Add(t => t.Description, "example"));

        // Assert
        Assert.AreEqual("E", comp.Find("button").GetAttribute("aria-label"));
    }

    [TestMethod]
    public void AriaLabel_WithEmptyTitleAndNoContent_ReturnsQuestionMarkInitial()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, string.Empty));

        // Assert
        Assert.AreEqual("?", comp.Find("button").GetAttribute("aria-label"));
    }

    [TestMethod]
    public void AbbreviationKey_WithTitle_UsesTitleForPaletteColor()
    {
        // Arrange
        var ctx = new BunitContext();
        var expected = TileAbbreviationPalette.GetColor("Mountain Retreat");

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, "Mountain Retreat"));

        // Assert
        StringAssert.Contains(comp.Find(".tile-avatar").GetAttribute("style"), $"background-color: {expected}");
    }

    [TestMethod]
    public void AbbreviationKey_WithWhitespaceTitle_UsesDescriptionForPaletteColor()
    {
        // Arrange
        var ctx = new BunitContext();
        var expected = TileAbbreviationPalette.GetColor("A quiet cabin");

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "   ")
            .Add(t => t.Description, "A quiet cabin"));

        // Assert
        StringAssert.Contains(comp.Find(".tile-avatar").GetAttribute("style"), $"background-color: {expected}");
    }

    [TestMethod]
    public void AbbreviationKey_WithNoTitleOrDescription_UsesInitialsForPaletteColor()
    {
        // Arrange
        var ctx = new BunitContext();
        var expected = TileAbbreviationPalette.GetColor("?");

        // Act
        var comp = ctx.Render<Tile>(p => p.Add(t => t.Title, string.Empty));

        // Assert
        StringAssert.Contains(comp.Find(".tile-avatar").GetAttribute("style"), $"background-color: {expected}");
    }
}
