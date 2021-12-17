//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System.Collections.Generic;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [mst.TestClass]
    public class SpinnerTypeTests
    {
        [mst.TestMethod]
        public void Render_PulseType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.Pulse));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-pulse""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_SquareType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.Square));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-square""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_DualRingType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.DualRing));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-dualring""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_HourglassType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.Hourglass));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-hourglass""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_IosSpinnerType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.SpinIOS));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-ios"">
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
