namespace D20Tek.BlazorComponents.UnitTests.ResultAlerts;

[TestClass]
public sealed class ResultAlertTests : BunitContext
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
    public void Render_ErrorFormatter_ShapesDefaultErrorLines()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(Error.Validation("Name", "Required.")))
            .Add(p => p.ErrorFormatter, e => $"{e.Code}: {e.Message}"));

        // assert
        Assert.Contains("Name: Required.", cut.Markup);
    }

    [TestMethod]
    public void Render_MaxErrorsShown_TruncatesAndShowsOverflow()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("A", "err a"),
                Error.Validation("B", "err b"),
                Error.Validation("C", "err c")))
            .Add(p => p.MaxErrorsShown, 1));

        // assert
        Assert.Contains("err a", cut.Markup);
        Assert.DoesNotContain("err b", cut.Markup);
        Assert.Contains("+2 more", cut.Markup);
    }

    [TestMethod]
    public void Render_GroupErrorsByCode_CollapsesDuplicates()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("Name", "first"),
                Error.Validation("Name", "second")))
            .Add(p => p.GroupErrorsByCode, true));

        // assert
        var items = cut.FindAll(".result-alert__error-item");
        Assert.HasCount(1, items);
        Assert.Contains("first", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowOnSuccessFalse_HidesSuccessAlert()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowOnSuccess, false));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_ShowOnFailureFalse_HidesErrorAlert()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult())
            .Add(p => p.ShowOnFailure, false));

        // assert
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_VariantOverride_OverridesAutoVariant()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Variant, AlertVariant.Warning));

        // assert
        Assert.Contains("result-alert-warning", cut.Markup);
        Assert.DoesNotContain("result-alert-success", cut.Markup);
    }

    [TestMethod]
    public void Render_SuccessVariantOverride_AppliesOnSuccess()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.SuccessVariant, AlertVariant.Info));

        // assert
        Assert.Contains("result-alert-info", cut.Markup);
    }

    [TestMethod]
    public void Render_FailureVariantOverride_AppliesOnFailure()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult())
            .Add(p => p.FailureVariant, AlertVariant.Neutral));

        // assert
        Assert.Contains("result-alert-neutral", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowCloseButton_RendersButtonAndDismissFiresCallback()
    {
        // arrange
        var dismissed = false;
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowCloseButton, true)
            .Add(p => p.OnDismiss, () => dismissed = true));

        // act
        cut.Find(".result-alert__close-btn").Click();

        // assert
        Assert.IsTrue(dismissed);
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_SizeAndModifiers_ProduceCssClasses()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Size, Size.Large)
            .Add(p => p.Bordered, true)
            .Add(p => p.Elevated, true));

        // assert
        Assert.Contains("result-alert-lg", cut.Markup);
        Assert.Contains("result-alert-bordered", cut.Markup);
        Assert.Contains("result-alert-elevated", cut.Markup);
    }

    [TestMethod]
    public void Render_AnimateDefault_ProducesAnimatedClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult()));

        // assert
        Assert.Contains("result-alert-animated", cut.Markup);
    }

    [TestMethod]
    public void Render_AnimateFalse_OmitsAnimatedClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Animate, false));

        // assert
        Assert.DoesNotContain("result-alert-animated", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowIconFalse_HidesIcon()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowIcon, false));

        // assert
        Assert.IsEmpty(cut.FindAll(".result-alert__icon"));
    }

    [TestMethod]
    public void Render_RemainingAttributes_MergesClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .AddUnmatched("class", "extra-class"));

        // assert
        Assert.Contains("extra-class", cut.Markup);
        Assert.Contains("result-alert", cut.Markup);
    }

    [TestMethod]
    public void Render_CustomCloseButtonAriaLabel_IsApplied()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowCloseButton, true)
            .Add(p => p.CloseButtonAriaLabel, "Close alert"));

        // assert
        Assert.Contains("aria-label=\"Close alert\"", cut.Markup);
    }

    [TestMethod]
    public void OnResultRendered_FiresOnStateTransition()
    {
        // arrange
        Result<TestModel>? captured = null;

        // act
        Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.OnResultRendered, r => captured = r));

        // assert
        Assert.IsNotNull(captured);
        Assert.IsTrue(captured.IsSuccess);
    }

    [TestMethod]
    public void OnSuccessShown_FiresForSuccessResult()
    {
        // arrange
        var shown = false;

        // act
        Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.OnSuccessShown, () => shown = true));

        // assert
        Assert.IsTrue(shown);
    }

    [TestMethod]
    public void OnErrorsShown_FiresForFailureResult()
    {
        // arrange
        IReadOnlyList<Error>? captured = null;

        // act
        Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(Error.Validation("Name", "Required.")))
            .Add(p => p.OnErrorsShown, errors => captured = errors));

        // assert
        Assert.IsNotNull(captured);
        Assert.HasCount(1, captured);
    }

    [TestMethod]
    public void GetErrors_NullResult_ReturnsEmpty()
    {
        // arrange
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, (Result<TestModel>?)null));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.IsEmpty(errors);
    }

    [TestMethod]
    public void GetErrors_SuccessResult_ReturnsEmpty()
    {
        // arrange
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult()));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.IsEmpty(errors);
    }

    [TestMethod]
    public void GetErrors_FailureResult_ReturnsErrors()
    {
        // arrange
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("Name", "Required."),
                Error.Validation("Email", "Invalid."))));

        // act
        var errors = cut.Instance.GetErrors();

        // assert
        Assert.HasCount(2, errors);
    }

    [TestMethod]
    public void Render_ErrorListLargerThanMax_ShowsTruncatedItemsAndOverflowCount()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("A", "err a"),
                Error.Validation("B", "err b"),
                Error.Validation("C", "err c"),
                Error.Validation("D", "err d")))
            .Add(p => p.MaxErrorsShown, 2));

        // assert
        var items = cut.FindAll(".result-alert__error-item");
        Assert.HasCount(2, items);
        Assert.Contains("err a", cut.Markup);
        Assert.Contains("err b", cut.Markup);
        Assert.DoesNotContain("err c", cut.Markup);
        Assert.DoesNotContain("err d", cut.Markup);
        Assert.Contains("+2 more", cut.Markup);
    }

    [TestMethod]
    public void Render_ErrorListWithinMax_ShowsNoOverflow()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult(
                Error.Validation("A", "err a"),
                Error.Validation("B", "err b")))
            .Add(p => p.MaxErrorsShown, 5));

        // assert
        var items = cut.FindAll(".result-alert__error-item");
        Assert.HasCount(2, items);
        Assert.Contains("err a", cut.Markup);
        Assert.Contains("err b", cut.Markup);
        Assert.DoesNotContain("more", cut.Markup);
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
