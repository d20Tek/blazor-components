namespace D20Tek.BlazorComponents.UnitTests.Timer;

[TestClass]
public partial class TimerOtherTests
{
    [TestMethod]
    public void OnTimerChanged()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>();

        // act
        comp.Instance.OnTimerChanged(true);

        // assert
        Assert.AreEqual(29, comp.Instance.TimeRemaining);
    }

    [TestMethod]
    public void ResetTimer()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>();

        // act
        comp.Instance.ResetTimer();

        // assert
        Assert.AreEqual(30, comp.Instance.TimeRemaining);
    }

    [TestMethod]
    [SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "testing parameter exception.")]
    public void SetOutOfRangeDuration()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>();

        // act - assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>
            comp.Instance.TimerDuration = -3);
    }

    [TestMethod]
    [SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "testing parameter exception.")]
    public void SetOutOfRangeWarning()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>();

        // act -assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>
            comp.Instance.TimerDuration = 1111000);

        // assert
    }

    [TestMethod]
    public void OnTimerChanged_ToZero()
    {
        // arrange
        var eventCalled = false;
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>(parameters =>
            parameters.Add(p => p.TimerDuration, 1)
                      .Add(p => p.TimerExpired, () => { eventCalled = true; }));

        // act
        comp.Instance.OnTimerChanged(true);

        // assert
        Assert.AreEqual(0, comp.Instance.TimeRemaining);
        Assert.IsTrue(eventCalled);
    }

    [TestMethod]
    public void Dispose()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<BlazorComponents.Timer>();

        // act
        comp.Instance.Dispose();

        // assert
        Assert.IsTrue(comp.Instance.IsDisposed);
    }

    [TestMethod]
    public void Dispose_WithNullTimer()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = new BlazorComponents.Timer();

        // act
        comp.Dispose();

        // assert
        Assert.IsTrue(comp.IsDisposed);
    }
}
