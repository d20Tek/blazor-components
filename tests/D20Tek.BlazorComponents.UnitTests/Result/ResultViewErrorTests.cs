namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultViewTests
{
    [TestMethod]
    public void GetErrors_NullResult_ReturnsEmpty()
    {
        // arrange
        var cut = Render<ResultView<TestModel>>(parameters => parameters.Add(p => p.Result, (Result<TestModel>?)null));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.IsEmpty(errors);
    }

    [TestMethod]
    public void GetErrors_SuccessResult_ReturnsEmpty()
    {
        // arrange
        var cut = Render<ResultView<TestModel>>(parameters => parameters.Add(p => p.Result, SuccessResult()));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.IsEmpty(errors);
    }

    [TestMethod]
    public void GetErrors_FailureResult_ReturnsErrors()
    {
        // arrange
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("Name", "Name is required."),
                Error.Validation("Email", "Email is invalid.")))
            .Add(p => p.Failure, errors => builder => builder.AddMarkupContent(0, "failure")));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.HasCount(2, errors);
    }
}
