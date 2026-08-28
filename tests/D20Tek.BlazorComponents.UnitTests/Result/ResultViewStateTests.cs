namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultViewTests
{
    [TestMethod]
    public void OnStateChanged_InvokedWithCurrentState()
    {
        // arrange
        var states = new List<ResultViewState>();

        // act
        Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Success, value => builder => builder.AddMarkupContent(0, "ok"))
            .Add(p => p.OnStateChanged, EventCallback.Factory.Create<ResultViewState>(this, states.Add)));

        // assert
        Assert.HasCount(1, states);
        Assert.AreEqual(ResultViewState.Success, states[0]);
    }

    [TestMethod]
    public void OnStateChanged_NullResultNotLoading_DoesNotReportInitialEmptyState()
    {
        // arrange
        var states = new List<ResultViewState>();

        // act
        Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, (Result<TestModel>?)null)
            .Add(p => p.OnStateChanged, EventCallback.Factory.Create<ResultViewState>(this, states.Add)));

        // assert - defaults already match the empty state, so no transition is reported
        Assert.IsEmpty(states);
    }

    [TestMethod]
    public void OnStateChanged_TransitionToEmpty_ReportsEmptyState()
    {
        // arrange
        var states = new List<ResultViewState>();
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Success, value => builder => builder.AddMarkupContent(0, "ok"))
            .Add(p => p.OnStateChanged, EventCallback.Factory.Create<ResultViewState>(this, states.Add)));

        // act
        cut.Render(parameters => parameters.Add(p => p.Result, (Result<TestModel>?)null));

        // assert
        Assert.AreEqual(ResultViewState.Empty, states[^1]);
    }

    [TestMethod]
    public void OnStateChanged_NullResultWhileLoading_ReportsLoadingNotEmpty()
    {
        // arrange
        var states = new List<ResultViewState>();

        // act
        Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.Result, (Result<TestModel>?)null)
            .Add(p => p.Loading, builder => builder.AddMarkupContent(0, "loading"))
            .Add(p => p.OnStateChanged, EventCallback.Factory.Create<ResultViewState>(this, states.Add)));

        // assert
        Assert.HasCount(1, states);
        Assert.AreEqual(ResultViewState.Loading, states[0]);
    }

    [TestMethod]
    public void OnStateChanged_InvokedWhenResultTransitions()
    {
        // arrange
        var states = new List<ResultViewState>();
        var cut = Render<ResultView<TestModel>>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.Loading, builder => builder.AddMarkupContent(0, "loading"))
            .Add(p => p.Success, value => builder => builder.AddMarkupContent(0, "ok"))
            .Add(p => p.OnStateChanged, EventCallback.Factory.Create<ResultViewState>(this, states.Add)));

        // act
        cut.Render(parameters => parameters
            .Add(p => p.IsLoading, false)
            .Add(p => p.Result, SuccessResult()));

        // assert
        Assert.AreEqual(ResultViewState.Loading, states[0]);
        Assert.AreEqual(ResultViewState.Success, states[^1]);
    }
}
