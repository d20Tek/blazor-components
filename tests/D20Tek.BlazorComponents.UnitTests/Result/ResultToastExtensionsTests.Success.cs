namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultToastExtensionsTests
{
    [TestMethod]
    public void ShowResult_Success_UsesSuccessVariantAndTimeout()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult());

        // assert
        Assert.IsNotNull(service.LastToast);
        Assert.AreEqual(NotificationVariant.Success, service.LastToast.Variant);
        Assert.AreEqual(TimeSpan.FromSeconds(3), service.LastToast.Timeout);
        Assert.IsFalse(service.LastToast.IsSticky);
    }

    [TestMethod]
    public void ShowResult_Success_DefaultMessage()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult());

        // assert
        Assert.Contains("Operation completed successfully.", RenderContent(service.LastToast!));
    }

    [TestMethod]
    public void ShowResult_Success_UsesSuccessMessageOverride()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult(), o => o.SuccessMessage = "Saved!");

        // assert
        Assert.Contains("Saved!", RenderContent(service.LastToast!));
    }

    [TestMethod]
    public void ShowResult_Success_UsesSuccessFormatter()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult(), o => o.SuccessFormatter = m => $"Hello {m.Name}");

        // assert
        Assert.Contains("Hello Ada", RenderContent(service.LastToast!));
    }

    [TestMethod]
    public void ShowResult_Success_MessageTakesPrecedenceOverFormatter()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult(), o =>
        {
            o.SuccessMessage = "Explicit";
            o.SuccessFormatter = [ExcludeFromCodeCoverage](m) => $"Hello {m.Name}";
        });

        // assert
        var markup = RenderContent(service.LastToast!);
        Assert.Contains("Explicit", markup);
        Assert.DoesNotContain("Hello Ada", markup);
    }
}
