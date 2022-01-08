//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System.Collections.Generic;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [mst.TestClass]
    public class SpinnerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>();

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }

        [mst.TestMethod]
        public void Render_WithInnerDivs()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label")
                .Add(p => p.Type, SpinType.SpinIOS));

            // assert
            var expectedHtml = @"
              <div class=""spinner-grid-container"">
                <div role=""status"" class=""spinner-ios spinner-area-main"">
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                    <div></div><div></div><div></div><div></div><div></div><div></div>
                </div>
                <div class=""spinner-label spinner-label-bottom"">Test label</div>
              </div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithAttributeSplat()
        {
            // arrange
            var ctx = new TestContext();
            var attr = new Dictionary<string, object>
            {
                { "style", "color: red; height: 120px; width 120 px" },
                { "disabled", true}
            };

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.RemainingAttributes, attr));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner"" style=""color: red; height: 120px; width 120 px"" disabled=""""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithAdditionalCssClasses()
        {
            // arrange
            var ctx = new TestContext();
            var attr = new Dictionary<string, object>
            {
                { "class", "test-component text-white" }
            };

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.RemainingAttributes, attr));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner test-component text-white""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_Colors()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Color, "green")
                .Add(p => p.SecondaryColor, "yellow"));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner"" style=""color: green; --spinner-secondary-color: yellow""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_NonDefaultSize()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Size, Size.Large));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner"" style=""--spinner-width: 8rem; --spinner-height: 8rem;""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_NoneSize()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Size, Size.None));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner""></div>";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
