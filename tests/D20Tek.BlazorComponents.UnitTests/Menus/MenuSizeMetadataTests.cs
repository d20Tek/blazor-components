namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class MenuSizeMetadataTests
{
    [TestMethod]
    [DataRow(Size.None, "")]
    [DataRow(Size.ExtraSmall, "d20tek-flyout-menu--xs")]
    [DataRow(Size.Small, "d20tek-flyout-menu--sm")]
    [DataRow(Size.Medium, "d20tek-flyout-menu--md")]
    [DataRow(Size.Large, "d20tek-flyout-menu--lg")]
    [DataRow(Size.ExtraLarge, "d20tek-flyout-menu--xl")]
    public void GetSizeCss_ForEachSize_ReturnsExpectedClass(Size size, string expected)
    {
        // Arrange & Act
        var result = MenuSizeMetadata.GetSizeCss(size);

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [ExcludeFromCodeCoverage]
    public void GetSizeCss_WithUndefinedSize_Throws()
    {
        // Act & Assert
        Assert.ThrowsExactly<KeyNotFoundException>(() => MenuSizeMetadata.GetSizeCss((Size)999));
    }
}
