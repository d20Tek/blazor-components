namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultToastExtensionsTests
{
    [TestMethod]
    public void ShowResult_Failure_UsesErrorVariantAndStickyTimeout()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(FailureResult());

        // assert
        Assert.IsNotNull(service.LastToast);
        Assert.AreEqual(NotificationVariant.Error, service.LastToast.Variant);
        Assert.AreEqual(TimeSpan.Zero, service.LastToast.Timeout);
        Assert.IsTrue(service.LastToast.IsSticky);
    }

    [TestMethod]
    public void ShowResult_Failure_RendersHeaderAndErrors()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(FailureResult(), o => o.FailureMessage = "Failed to save.");

        // assert
        var markup = RenderContent(service.LastToast!);
        Assert.Contains("Failed to save.", markup);
        Assert.Contains("First error", markup);
    }

    [TestMethod]
    public void ShowResult_Failure_UsesErrorFormatter()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(FailureResult(), o => o.ErrorFormatter = e => $"[{e.Code}] {e.Message}");

        // assert
        Assert.Contains("[E1] First error", RenderContent(service.LastToast!));
    }

    [TestMethod]
    public void ShowResult_Failure_GroupErrorsByCode_CollapsesDuplicates()
    {
        // arrange
        var service = new FakeToastService();
        var result = FailureResult(
            Error.Validation("DUP", "Message one"),
            Error.Validation("DUP", "Message two"));

        // act
        service.ShowResult(result, o => o.GroupErrorsByCode = true);

        // assert
        var markup = RenderContent(service.LastToast!);
        Assert.Contains("Message one", markup);
        Assert.DoesNotContain("Message two", markup);
    }

    [TestMethod]
    public void ShowResult_Failure_MaxErrorsShown_LimitsErrors()
    {
        // arrange
        var service = new FakeToastService();
        var result = FailureResult(
            Error.Validation("E1", "Error one"),
            Error.Validation("E2", "Error two"),
            Error.Validation("E3", "Error three"));

        // act
        service.ShowResult(result, o => o.MaxErrorsShown = 2);

        // assert
        var markup = RenderContent(service.LastToast!);
        Assert.Contains("Error one", markup);
        Assert.Contains("Error two", markup);
        Assert.DoesNotContain("Error three", markup);
    }
}
