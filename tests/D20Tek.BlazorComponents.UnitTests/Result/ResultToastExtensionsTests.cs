namespace D20Tek.BlazorComponents.UnitTests.Result;

[TestClass]
public sealed partial class ResultToastExtensionsTests
{
    private sealed record TestModel(string Name);

    private static Result<TestModel> SuccessResult() => Result<TestModel>.Success(new TestModel("Ada"));

    private static Result<TestModel> FailureResult(params Error[] errors) =>
        Result<TestModel>.Failure(errors.Length == 0 ? [Error.Validation("E1", "First error")] : errors);

    private static string RenderContent(ToastInstance toast) => new BunitContext().Render(toast.Content).Markup;

    [TestMethod]
    public void ShowResult_NullService_Throws()
    {
        // arrange
        IToastService service = null!;

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage]() => service.ShowResult(SuccessResult()));
    }

    [TestMethod]
    public void ShowResult_NullResult_Throws()
    {
        // arrange
        var service = new FakeToastService();

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>(
            [ExcludeFromCodeCoverage]() => service.ShowResult<TestModel>(null!));
    }

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
        Assert.AreEqual(TimeSpan.FromSeconds(5), service.LastToast.Timeout);
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

    [TestMethod]
    public void ShowResult_AppliesPositionAndDisplayOptions()
    {
        // arrange
        var service = new FakeToastService();

        // act
        service.ShowResult(SuccessResult(), o =>
        {
            o.Position = ToastPosition.TopCenter;
            o.ShowIcon = false;
            o.Dismissible = false;
            o.Animate = false;
        });

        // assert
        var toast = service.LastToast!;
        Assert.AreEqual(ToastPosition.TopCenter, toast.Position);
        Assert.IsFalse(toast.ShowIcon);
        Assert.IsFalse(toast.Dismissible);
        Assert.IsFalse(toast.Animate);
    }

    [TestMethod]
    public void ShowResult_ReturnsToastInstance()
    {
        // arrange
        var service = new FakeToastService();

        // act
        var toast = service.ShowResult(SuccessResult());

        // assert
        Assert.IsNotNull(toast);
        Assert.AreEqual(service.LastToast!.Id, toast.Id);
    }
}
