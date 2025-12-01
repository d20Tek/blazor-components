namespace D20Tek.BlazorComponents.UnitTests.Timer;

[TestClass]
public partial class TimerTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>();

        // assert
        var results = comp.CompareTo(TimerTests.Expected.Default);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithParameters()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>(parameters =>
            parameters.Add(p => p.TimerDuration, 60)
                      .Add(p => p.WarningThreshold, 30)
                      .Add(p => p.AlertThreshold, 15)
                      .Add(p => p.ElapsedTimeColor, "lightgreen")
                      .Add(p => p.RemainingTimeColor, "darkgray")
                      .Add(p => p.WarningTimeColor, "pink")
                      .Add(p => p.AlertTimeColor, "purple"));

        // assert
        var results = comp.CompareTo(TimerTests.Expected.WithParameters);
        TimerVerifier.VerifyMarkupDifferences(results);

        Assert.AreEqual(60, comp.Instance.TimerDuration);
        Assert.AreEqual(30, comp.Instance.WarningThreshold);
        Assert.AreEqual(15, comp.Instance.AlertThreshold);
        Assert.AreEqual("lightgreen", comp.Instance.ElapsedTimeColor);
        Assert.AreEqual("darkgray", comp.Instance.RemainingTimeColor);
        Assert.AreEqual("pink", comp.Instance.WarningTimeColor);
        Assert.AreEqual("purple", comp.Instance.AlertTimeColor);
    }

    [TestMethod]
    public void Render_WithWarningColor()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>(parameters => parameters.Add(p => p.WarningThreshold, 30));

        // assert
        var results = comp.CompareTo(TimerTests.Expected.WithWarningColor);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithExpirationMessage()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>(parameters =>
            parameters.Add(p => p.TimerDuration, 0)
                      .Add(p => p.ExpirationMessage, "Test!"));

        // assert
        var results = comp.CompareTo(TimerTests.Expected.WithExpirationMessage);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithSizeLarge()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>(parameters => parameters.Add(p => p.Size, Size.Large));

        // assert
        var results = comp.CompareTo(TimerTests.Expected.WithSizeLarge);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithSizeSmall()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.Timer>(parameters => parameters.Add(p => p.Size, Size.Small));

        // assert
        var results = comp.CompareTo(TimerTests.Expected.WithSizeSmall);
        TimerVerifier.VerifyMarkupDifferences(results);
    }
}
