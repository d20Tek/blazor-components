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
