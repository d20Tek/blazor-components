namespace D20Tek.BlazorComponents.UnitTests.Result;

[TestClass]
public sealed partial class ResultViewTests : BunitContext
{
    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    private static Result<TestModel> SuccessResult() =>
        Result<TestModel>.Success(new TestModel { Name = "Ada", Email = "ada@example.com" });

    private static Result<TestModel> FailureResult(params Error[] errors) =>
        Result<TestModel>.Failure(errors.Length == 0 ? [Error.Validation("Name", "Required.")] : errors);

    [TestMethod]
    public void Render_Loading_ShowsLoadingContent()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.Loading, builder => builder.AddMarkupContent(0, "<p>loading...</p>"))
            .Add(p => p.Success, [ExcludeFromCodeCoverage](value) => builder => builder.AddMarkupContent(0, "success"))
            .Add(p => p.Failure, [ExcludeFromCodeCoverage](errors) => builder => builder.AddMarkupContent(0, "failure")));

        // assert
        Assert.Contains("loading...", cut.Markup);
        Assert.DoesNotContain("success", cut.Markup);
        Assert.DoesNotContain("failure", cut.Markup);
    }

    [TestMethod]
    public void Render_LoadingTakesPrecedenceOverResult()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Loading, builder => builder.AddMarkupContent(0, "<p>loading...</p>"))
            .Add(p => p.Success, [ExcludeFromCodeCoverage](value) => builder => builder.AddMarkupContent(0, $"Hi {value.Name}")));

        // assert
        Assert.Contains("loading...", cut.Markup);
        Assert.DoesNotContain("Hi Ada", cut.Markup);
    }

    [TestMethod]
    public void Render_NullResult_RendersEmptyContent()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, (Result<TestModel>?)null)
            .Add(p => p.Empty, builder => builder.AddMarkupContent(0, "<p>nothing yet</p>")));

        // assert
        Assert.Contains("nothing yet", cut.Markup);
    }

    [TestMethod]
    public void Render_NullResultWithoutEmptyContent_RendersNothing()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters.Add(p => p.Result, (Result<TestModel>?)null));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_Success_RendersSuccessContentWithValue()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Success, value => builder => builder.AddMarkupContent(0, $"<span>Hi {value.Name}</span>"))
            .Add(p => p.Failure, [ExcludeFromCodeCoverage](errors) => builder => builder.AddMarkupContent(0, "failure")));

        // assert
        Assert.Contains("Hi Ada", cut.Markup);
        Assert.DoesNotContain("failure", cut.Markup);
    }

    [TestMethod]
    public void Render_Failure_RendersFailureContentWithErrors()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("Name", "Name is required."),
                Error.Validation("Email", "Email is invalid.")))
            .Add(p => p.Success, [ExcludeFromCodeCoverage] (value) => builder => builder.AddMarkupContent(0, "success"))
            .Add(p => p.Failure, errors => builder =>
            {
                builder.AddMarkupContent(0, $"<span>{errors.Count} problems</span>");
                foreach (var error in errors)
                {
                    builder.AddMarkupContent(1, $"<li>{error.Message}</li>");
                }
            }));

        // assert
        Assert.Contains("2 problems", cut.Markup);
        Assert.Contains("Name is required.", cut.Markup);
        Assert.Contains("Email is invalid.", cut.Markup);
        Assert.DoesNotContain("success", cut.Markup);
    }

    [TestMethod]
    public void Render_FailureWithoutFailureContent_RendersDefaultFailureAlert()
    {
        // arrange - act
        var cut = Render<ResultView<TestModel>>(parameters => parameters.Add(p => p.Result, FailureResult()));

        // assert
        Assert.Contains("An error has occurred.", cut.Markup);
        Assert.Contains("result-view__failure", cut.Markup);
        Assert.Contains("role=\"alert\"", cut.Markup);
    }
}
