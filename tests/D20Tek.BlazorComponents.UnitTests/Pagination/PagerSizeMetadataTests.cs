namespace D20Tek.BlazorComponents.UnitTests.Pagination;

[TestClass]
public sealed class PagerSizeMetadataTests
{
    [TestMethod]
    [DataRow(Size.None, "")]
    [DataRow(Size.ExtraSmall, "pager-xs")]
    [DataRow(Size.Small, "pager-sm")]
    [DataRow(Size.Medium, "pager-md")]
    [DataRow(Size.Large, "pager-lg")]
    [DataRow(Size.ExtraLarge, "pager-xl")]
    public void GetSizeCss_KnownSize_ReturnsExpectedCssClass(Size size, string expected)
    {
        // arrange - act
        var actual = PagerSizeMetadata.GetSizeCss(size);

        // assert
        Assert.AreEqual(expected, actual);
    }
}
