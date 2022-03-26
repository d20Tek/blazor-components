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

    public class CountdownTimerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<CountdownTimer>();

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  ...
</div>
";

            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithCountdownTarget()
        {
            // arrange
            var ctx = new TestContext();
            var time = DateTimeOffset.Now.AddHours(1);

            // act
            var comp = ctx.RenderComponent<CountdownTimer>(parameters =>
                parameters.Add(p => p.CountdownTarget, time)
                          .Add(p => p.ExpirationMessage, "Test Expired"));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  ...
</div>
";

            comp.MarkupMatches(expectedHtml);
            mst.Assert.AreEqual(time, comp.Instance.CountdownTarget);
            mst.Assert.AreEqual("Test Expired", comp.Instance.ExpirationMessage);
        }

        [mst.TestMethod]
        public void Render_WithLabelText()
        {
            // arrange
            var ctx = new TestContext();
            var time = DateTimeOffset.Now.AddHours(1);

            // act
            var comp = ctx.RenderComponent<CountdownTimer>(parameters =>
                parameters.Add(p => p.LabelText, "My timer:"));

            // assert
            var expectedHtml = @$"
<div role=""timer"" class=""base-timer"">
  <label class=""base-timer-label"">My timer:&nbsp;</label>
  ...
</div>
";

            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void OnTimerChanged()
        {
            // arrange
            var ctx = new TestContext();
            var comp = ctx.RenderComponent<CountdownTimer>(parameters =>
                parameters.Add(p => p.CountdownTarget, DateTimeOffset.Now.AddSeconds(10)));

            // act
            comp.Instance.OnTimerChanged(true);

            // assert
            mst.Assert.AreEqual("0:00:09", comp.Instance.TimerDisplay);
        }
    }
}
