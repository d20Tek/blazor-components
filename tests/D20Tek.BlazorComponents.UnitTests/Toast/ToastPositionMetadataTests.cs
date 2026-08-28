namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastPositionMetadataTests
{
    [TestMethod]
    [DataRow(ToastPosition.TopLeft, "toast-host-top-left")]
    [DataRow(ToastPosition.TopCenter, "toast-host-top-center")]
    [DataRow(ToastPosition.TopRight, "toast-host-top-right")]
    [DataRow(ToastPosition.MiddleLeft, "toast-host-middle-left")]
    [DataRow(ToastPosition.MiddleCenter, "toast-host-middle-center")]
    [DataRow(ToastPosition.MiddleRight, "toast-host-middle-right")]
    [DataRow(ToastPosition.BottomLeft, "toast-host-bottom-left")]
    [DataRow(ToastPosition.BottomCenter, "toast-host-bottom-center")]
    [DataRow(ToastPosition.BottomRight, "toast-host-bottom-right")]
    public void GetPositionCss_ReturnsExpectedClass(ToastPosition position, string expected)
    {
        // arrange - act
        var css = ToastPositionMetadata.GetPositionCss(position);

        // assert
        Assert.AreEqual(expected, css);
    }
}
