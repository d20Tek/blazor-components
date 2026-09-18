namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileLayoutMetadataTests
{
    [TestMethod]
    [DataRow(LayoutOption.Compact, "tile-layout-compact")]
    [DataRow(LayoutOption.Common, "tile-layout-common")]
    [DataRow(LayoutOption.Verbose, "tile-layout-verbose")]
    public void GetLayoutCss_KnownLayout_ReturnsExpectedModifier(LayoutOption layout, string expected)
    {
        // Arrange

        // Act
        var result = TileLayoutMetadata.GetLayoutCss(layout);

        // Assert
        Assert.AreEqual(expected, result);
    }
}
