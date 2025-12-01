namespace D20Tek.BlazorComponents.UnitTests.Timer;

[TestClass]
public class SpanTimerTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<SpanTimer>();

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">0:30</div>
  </div>
</div>
";

        var results = comp.CompareTo(expectedHtml);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithMinuteSpan()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<SpanTimer>(parameters => 
            parameters.Add(p => p.TimerDurationSpan, new TimeSpan(0, 1, 0)));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">1:00</div>
  </div>
</div>
";

        var results = comp.CompareTo(expectedHtml);
        TimerVerifier.VerifyMarkupDifferences(results);
    }

    [TestMethod]
    public void Render_WithHourSpan()
    {
        // arrange
        var ctx = new BunitContext();
        var span = new TimeSpan(1, 10, 13);

        // act
        var comp = ctx.Render<SpanTimer>(parameters => parameters.Add(p => p.TimerDurationSpan, span));

        // assert
        var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: green"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">1:10:13</div>
  </div>
</div>
";

        var results = comp.CompareTo(expectedHtml);
        TimerVerifier.VerifyMarkupDifferences(results);

        Assert.AreEqual(span, comp.Instance.TimerDurationSpan);
    }

    [TestMethod]
    [SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "testing parameter exception.")]
    public void SetOutOfRangeDurationSpan()
    {
        // arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<SpanTimer>();

        // act - assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>
            comp.Instance.TimerDurationSpan = new TimeSpan(500, 1, 15, 0));
    }
}
