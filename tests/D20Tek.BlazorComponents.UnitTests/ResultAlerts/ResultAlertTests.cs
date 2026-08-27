namespace D20Tek.BlazorComponents.UnitTests.ResultAlerts;

[TestClass]
public sealed partial class ResultAlertTests : BunitContext
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
    public void Render_NullResult_RendersNothing()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, (Result<TestModel>?)null));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_NullResultWithEmptyContent_RendersEmptyContent()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, (Result<TestModel>?)null)
            .Add(p => p.EmptyContent, builder => builder.AddMarkupContent(0, "<p>nothing yet</p>")));

        // assert
        Assert.Contains("nothing yet", cut.Markup);
    }

    [TestMethod]
    public void Render_Success_ShowsDefaultTitleAndSuccessVariant()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult()));

        // assert
        Assert.Contains("Success", cut.Markup);
        Assert.Contains("result-alert-success", cut.Markup);
        Assert.Contains("role=\"alert\"", cut.Markup);
        Assert.Contains("aria-live=\"polite\"", cut.Markup);
    }

    [TestMethod]
    public void Render_Failure_ShowsErrorMessagesAndErrorVariant()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("Name", "Name is required."),
                Error.Validation("Email", "Email is invalid."))));

        // assert
        Assert.Contains("Name is required.", cut.Markup);
        Assert.Contains("Email is invalid.", cut.Markup);
        Assert.Contains("result-alert-error", cut.Markup);
        Assert.Contains("aria-live=\"assertive\"", cut.Markup);
    }

    [TestMethod]
    public void Render_SuccessContent_ReceivesValue()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.SuccessContent, value => builder =>
                builder.AddMarkupContent(0, $"<span>Hi {value.Name}</span>")));

        // assert
        Assert.Contains("Hi Ada", cut.Markup);
    }

    [TestMethod]
    public void Render_ErrorContent_ReceivesErrors()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(Error.Validation("Name", "Required.")))
            .Add(p => p.ErrorContent, errors => builder =>
                builder.AddMarkupContent(0, $"<span>{errors.Count} problems</span>")));

        // assert
        Assert.Contains("1 problems", cut.Markup);
    }

    [TestMethod]
    public void Render_HeaderContent_RendersInHeaderSection()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.HeaderContent, builder =>
                builder.AddMarkupContent(0, "<span>Custom header</span>")));

        // assert
        var header = cut.Find(".result-alert__header");
        Assert.Contains("Custom header", header.InnerHtml);
    }

    [TestMethod]
    public void Render_FooterContent_RendersInFooterSection()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.FooterContent, builder =>
                builder.AddMarkupContent(0, "<button>Try again</button>")));

        // assert
        var footer = cut.Find(".result-alert__footer");
        Assert.Contains("Try again", footer.InnerHtml);
    }
}
