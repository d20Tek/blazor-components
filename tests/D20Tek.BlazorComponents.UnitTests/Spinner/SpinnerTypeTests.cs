using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [TestClass]
    public class SpinnerTypeTests
    {
        [TestMethod]
        public void Render_PulseType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Pulse));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-pulse""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_SquareType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Square));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-square""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_DualRingType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.DualRing));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-dualring""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_HourglassType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Hourglass));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-hourglass""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_IosSpinnerType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.SpinIOS));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-ios"">
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_RippleType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Ripple));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-ripple"">
                    <div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_RollerType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Roller));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-roller"">
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                    <div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_CircleType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Circle));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-circle"">
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_BlocksType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Blocks));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-blocks"">
                    <div></div><div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_EllipsisType()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Type, SpinType.Ellipsis));

            // assert
            var expectedHtml = @"
                <div role=""status"" class=""spinner-ellipsis"">
                    <div></div><div></div><div></div><div></div>
                </div>";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
