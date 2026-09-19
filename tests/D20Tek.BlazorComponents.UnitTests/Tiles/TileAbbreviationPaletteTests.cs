namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileAbbreviationPaletteTests
{
    [TestMethod]
    public void Count_Returns16Colors()
    {
        // Arrange

        // Act
        var result = TileAbbreviationPalette.Count;

        // Assert
        Assert.AreEqual(16, result);
    }

    [TestMethod]
    public void GetColor_SameKey_ReturnsSameColorDeterministically()
    {
        // Arrange
        const string key = "Mountain Retreat";

        // Act
        var first = TileAbbreviationPalette.GetColor(key);
        var second = TileAbbreviationPalette.GetColor(key);

        // Assert
        Assert.AreEqual(first, second);
    }

    [TestMethod]
    public void GetColor_NullKey_ReturnsFirstColor()
    {
        // Arrange

        // Act
        var result = TileAbbreviationPalette.GetColor(null);

        // Assert
        Assert.AreEqual("#5b6b8c", result);
    }

    [TestMethod]
    public void GetColor_EmptyKey_ReturnsFirstColor()
    {
        // Arrange

        // Act
        var result = TileAbbreviationPalette.GetColor(string.Empty);

        // Assert
        Assert.AreEqual("#5b6b8c", result);
    }

    [TestMethod]
    public void GetColor_AnyKey_ReturnsColorFromPalette()
    {
        // Arrange

        // Act
        var result = TileAbbreviationPalette.GetColor("arbitrary key value");

        // Assert
        Assert.StartsWith("#", result);
        Assert.AreEqual(7, result.Length);
    }
}
