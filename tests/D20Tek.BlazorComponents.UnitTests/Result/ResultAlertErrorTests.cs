namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultAlertTests
{
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
}
