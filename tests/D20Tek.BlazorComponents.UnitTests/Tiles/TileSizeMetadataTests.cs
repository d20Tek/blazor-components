namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileSizeMetadataTests
{
    [TestMethod]
    [DataRow(Size.None, "")]
    [DataRow(Size.ExtraSmall, "tile-xs")]
    [DataRow(Size.Small, "tile-sm")]
    [DataRow(Size.Medium, "tile-md")]
    [DataRow(Size.Large, "tile-lg")]
    [DataRow(Size.ExtraLarge, "tile-xl")]
    public void GetSizeCss_KnownSize_ReturnsExpectedModifier(Size size, string expected)
    {
        // Arrange

        // Act
        var result = TileSizeMetadata.GetSizeCss(size);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
