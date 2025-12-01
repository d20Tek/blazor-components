namespace D20Tek.BlazorComponents.UnitTests.Timer;

[TestClass]

public class CountdownTimerTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<CountdownTimer>();

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithCountdownTarget()
    {
        // arrange
        var ctx = new BunitContext();
        var time = DateTimeOffset.Now.AddHours(1);

        // act
        var comp = ctx.Render<CountdownTimer>(parameters =>
            parameters.Add(p => p.CountdownTarget, time)
                      .Add(p => p.ExpirationMessage, "Test Expired"));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
        Assert.AreEqual(time, comp.Instance.CountdownTarget);
        Assert.AreEqual("Test Expired", comp.Instance.ExpirationMessage);
    }

    [TestMethod]
    public void Render_WithLabelText()
    {
        // arrange
        var ctx = new BunitContext();
        var time = DateTimeOffset.Now.AddHours(1);

        // act
        var comp = ctx.Render<CountdownTimer>(parameters => parameters.Add(p => p.LabelText, "My timer:"));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  <label class=""base-timer-label"">My timer:&nbsp;</label>
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithSizeSmall()
    {
        // arrange
        var ctx = new BunitContext();
        var time = DateTimeOffset.Now.AddHours(1);

        // act
        var comp = ctx.Render<CountdownTimer>(parameters => parameters.Add(p => p.Size, Size.Small));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-sm"">
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithSizeMedium()
    {
        // arrange
        var ctx = new BunitContext();
        var time = DateTimeOffset.Now.AddHours(1);

        // act
        var comp = ctx.Render<CountdownTimer>(parameters => parameters.Add(p => p.Size, Size.Medium));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithSizeLarge()
    {
        // arrange
        var ctx = new BunitContext();
        var time = DateTimeOffset.Now.AddHours(1);

        // act
        var comp = ctx.Render<CountdownTimer>(parameters => parameters.Add(p => p.Size, Size.Large));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-lg"">
  ...
</div>
";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void OnTimerChanged()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<CountdownTimer>(parameters =>
            parameters.Add(p => p.CountdownTarget, DateTimeOffset.Now.AddSeconds(10)));

        // act
        comp.Instance.OnTimerChanged(true);

        // assert
        Assert.AreEqual("0:00:09", comp.Instance.TimerDisplay);
    }
}
