namespace D20Tek.BlazorComponents.UnitTests.ResultAlerts;

[TestClass]
public sealed class ResultAlertVariantMetadataTests
{
    [TestMethod]
    public void GetMetadataItem_ReturnsItemWithCssClassAndIcon()
    {
        // arrange - act
        var item = ResultAlertVariantMetadata.GetMetadataItem(AlertVariant.Success);

        // assert
        Assert.IsNotNull(item);
        Assert.AreEqual("result-alert-success", item.CssClass);
        Assert.Contains("svg", item.DefaultIcon);
    }

    [TestMethod]
    public void GetVariantCss_ReturnsExpectedCssClass()
    {
        // arrange - act
        var css = ResultAlertVariantMetadata.GetVariantCss(AlertVariant.Error);

        // assert
        Assert.AreEqual("result-alert-error", css);
    }

    [TestMethod]
    public void GetDefaultIcon_ReturnsIconMarkup()
    {
        // arrange - act
        var icon = ResultAlertVariantMetadata.GetDefaultIcon(AlertVariant.Warning);

        // assert
        Assert.Contains("svg", icon);
    }
}
