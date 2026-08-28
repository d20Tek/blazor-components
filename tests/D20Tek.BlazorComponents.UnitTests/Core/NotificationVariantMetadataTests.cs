namespace D20Tek.BlazorComponents.UnitTests.Core;

[TestClass]
public sealed class NotificationVariantMetadataTests
{
    [TestMethod]
    public void GetMetadataItem_ReturnsItemWithTokenAndIcon()
    {
        // arrange - act
        var item = NotificationVariantMetadata.GetMetadataItem(NotificationVariant.Success);

        // assert
        Assert.IsNotNull(item);
        Assert.AreEqual("success", item.Token);
        Assert.Contains("svg", item.DefaultIcon);
    }

    [TestMethod]
    public void GetVariantToken_ReturnsExpectedToken()
    {
        // arrange - act
        var token = NotificationVariantMetadata.GetVariantToken(NotificationVariant.Error);

        // assert
        Assert.AreEqual("error", token);
    }

    [TestMethod]
    public void GetDefaultIcon_ReturnsIconMarkup()
    {
        // arrange - act
        var icon = NotificationVariantMetadata.GetDefaultIcon(NotificationVariant.Warning);

        // assert
        Assert.Contains("svg", icon);
    }
}
