using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using Comp = D20Tek.BlazorComponents;

namespace D20Tek.BlazorComponents.UnitTests.Timer
{
    [TestClass]
    public class TimerTests
    {
        [TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Comp.Timer>();

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
        public void Render_WithParameters()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Comp.Timer>(parameters =>
                parameters.Add(p => p.TimerDuration, 60)
                          .Add(p => p.WarningThreshold, 30)
                          .Add(p => p.AlertThreshold, 15)
                          .Add(p => p.ElapsedTimeColor, "lightgreen")
                          .Add(p => p.RemainingTimeColor, "darkgray")
                          .Add(p => p.WarningTimeColor, "pink")
                          .Add(p => p.AlertTimeColor, "purple"));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: lightgreen""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: darkgray"" d=""
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
            var comp = ctx.Render<Comp.Timer>(parameters => parameters.Add(p => p.WarningThreshold, 30));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""283 283"" class=""base-timer__path-remaining""
            style=""stroke: orange"" d=""
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
        public void Render_WithExpirationMessage()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Comp.Timer>(parameters =>
                parameters.Add(p => p.TimerDuration, 0)
                          .Add(p => p.ExpirationMessage, "Test!"));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-md"">
  <svg class=""base-timer__svg"" viewBox=""0 0 100 100"" xmlns=""http://www.w3.org/2000/svg"">
    <g class=""base-timer__circle"">
      <circle class=""base-timer__path-elapsed"" cx=""50"" cy=""50"" r=""45"" style=""stroke: gray""></circle>
      <path id=""base-timer-path-remaining"" stroke-dasharray=""NaN 283"" class=""base-timer__path-remaining""
            style=""stroke: red"" d=""
              M 50, 50
              m -45, 0
              a 45,45 0 1,0 90,0
              a 45,45 0 1,0 -90,0
            ""></path>
    </g>
  </svg>
  <div id=""base-timer-label"" class=""base-timer__label"">
    <div class=""base-timer__label-inner"">Test!</div>
  </div>
</div>
";

            var results = comp.CompareTo(expectedHtml);
            TimerVerifier.VerifyMarkupDifferences(results);
        }

        [TestMethod]
        public void Render_WithSizeLarge()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Comp.Timer>(parameters => parameters.Add(p => p.Size, Size.Large));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-lg"">
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
        public void Render_WithSizeSmall()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Comp.Timer>(parameters => parameters.Add(p => p.Size, Size.Small));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer base-timer-sm"">
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
        public void OnTimerChanged()
        {
            // arrange
            var ctx = new BunitContext();
            var comp = ctx.Render<Comp.Timer>();

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
            var comp = ctx.Render<Comp.Timer>();

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
            var comp = ctx.Render<Comp.Timer>();

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
            var comp = ctx.Render<Comp.Timer>();

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
            var comp = ctx.Render<Comp.Timer>(parameters =>
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
            var comp = ctx.Render<Comp.Timer>();

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
            var comp = new Comp.Timer();

            // act
            comp.Dispose();

            // assert
            Assert.IsTrue(comp.IsDisposed);
        }
    }
}
