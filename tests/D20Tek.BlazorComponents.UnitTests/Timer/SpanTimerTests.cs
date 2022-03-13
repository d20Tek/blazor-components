//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System;
using System.Diagnostics.CodeAnalysis;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests.Timer
{
    [mst.TestClass]
    public class SpanTimerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<SpanTimer>();

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

        [mst.TestMethod]
        public void Render_WithMinuteSpan()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<SpanTimer>(parameters => 
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

        [mst.TestMethod]
        public void Render_WithHourSpan()
        {
            // arrange
            var ctx = new TestContext();
            var span = new TimeSpan(1, 10, 13);

            // act
            var comp = ctx.RenderComponent<SpanTimer>(parameters =>
                parameters.Add(p => p.TimerDurationSpan, span));

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

            mst.Assert.AreEqual(span, comp.Instance.TimerDurationSpan);
        }

        [mst.TestMethod]
        [ExcludeFromCodeCoverage]
        [mst.ExpectedException(typeof(ArgumentOutOfRangeException))]
        [SuppressMessage("Usage", "BL0005:Component parameter should not be set outside of its component.", Justification = "testing parameter exception.")]
        public void SetOutOfRangeDurationSpan()
        {
            // arrange
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<SpanTimer>();

            // act
            comp.Instance.TimerDurationSpan = new TimeSpan(500, 1, 15, 0);

            // assert
        }
    }
}
