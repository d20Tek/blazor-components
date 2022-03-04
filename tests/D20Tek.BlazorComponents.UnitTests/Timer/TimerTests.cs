//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using AngleSharp.Diffing.Core;
using Bunit;
using System;
using c = D20Tek.BlazorComponents;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests.Timer
{
    [mst.TestClass]
    public class TimerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>();

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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }

        [mst.TestMethod]
        public void Render_WithParameters()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);

            mst.Assert.AreEqual(60, comp.Instance.TimerDuration);
            mst.Assert.AreEqual(30, comp.Instance.WarningThreshold);
            mst.Assert.AreEqual(15, comp.Instance.AlertThreshold);
            mst.Assert.AreEqual("lightgreen", comp.Instance.ElapsedTimeColor);
            mst.Assert.AreEqual("darkgray", comp.Instance.RemainingTimeColor);
            mst.Assert.AreEqual("pink", comp.Instance.WarningTimeColor);
            mst.Assert.AreEqual("purple", comp.Instance.AlertTimeColor);
        }

        [mst.TestMethod]
        public void Render_WithWarningColor()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
                parameters.Add(p => p.WarningThreshold, 30));

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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }

        [mst.TestMethod]
        public void Render_WithExpirationMessage()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }

        [mst.TestMethod]
        public void Render_WithSizeLarge()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
                parameters.Add(p => p.Size, Size.Large));

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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }

        [mst.TestMethod]
        public void Render_WithSizeSmall()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
                parameters.Add(p => p.Size, Size.Small));

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
            mst.Assert.AreEqual(1, results.Count);
            var source = (AttrDiff)results[0];
            mst.Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }

        [mst.TestMethod]
        public void OnTimerChanged()
        {
            // arrange
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<c.Timer>();

            // act
            comp.Instance.OnTimerChanged(true);

            // assert
            mst.Assert.AreEqual(29, comp.Instance.TimeRemaining);
        }

        [mst.TestMethod]
        public void ResetTimer()
        {
            // arrange
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<c.Timer>();

            // act
            comp.Instance.ResetTimer();

            // assert
            mst.Assert.AreEqual(30, comp.Instance.TimeRemaining);
        }

        [mst.TestMethod]
        public void OnTimerChanged_ToZero()
        {
            // arrange
            var eventCalled = false;
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<c.Timer>(parameters =>
                parameters.Add(p => p.TimerDuration, 1)
                          .Add(p => p.TimerExpired, () => { eventCalled = true; }));

            // act
            comp.Instance.OnTimerChanged(true);

            // assert
            mst.Assert.AreEqual(0, comp.Instance.TimeRemaining);
            mst.Assert.IsTrue(eventCalled);
        }

        [mst.TestMethod]
        public void Dispose()
        {
            // arrange
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<c.Timer>();

            // act
            comp.Instance.Dispose();

            // assert
            mst.Assert.IsTrue(comp.Instance.IsDisposed);
        }

        [mst.TestMethod]
        public void Dispose_WithNullTimer()
        {
            // arrange
            var ctx = new TestContext();
            var comp = new c.Timer();

            // act
            comp.Dispose();

            // assert
            mst.Assert.IsTrue(comp.IsDisposed);
        }
    }
}
