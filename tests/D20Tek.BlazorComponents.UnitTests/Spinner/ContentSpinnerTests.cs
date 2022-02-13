//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System.Collections.Generic;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [mst.TestClass]
    public class ContentSpinnerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<ContentSpinner>();

            // assert
            var expectedHtml = @"<div role=""status"" class=""content-spinner""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<ContentSpinner>(parameters => parameters
                .Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }

        [mst.TestMethod]
        public void Render_WithChildContent()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<ContentSpinner>(parameters => parameters
                .AddChildContent("Test message..."));

            // assert
            var expectedHtml = @"<div role=""status"" class=""content-spinner"">Test message...</div>";
            comp.MarkupMatches(expectedHtml);
        }


        [mst.TestMethod]
        public void Render_WithImageChildContent()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<ContentSpinner>(parameters => parameters
                .AddChildContent(@"<img alt=""test image"" src=""./test/image.png"" />"));

            // assert
            var expectedHtml = @"
<div role=""status"" class=""content-spinner"">
    <img alt=""test image"" src=""./test/image.png"" />
</div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithSize()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<ContentSpinner>(parameters => parameters
                .Add(p => p.Size, Size.Medium)
                .AddChildContent("Test message..."));

            // assert
            var expectedHtml = @"
<div role=""status"" class=""content-spinner"" style=""--spinner-width: 4rem; --spinner-height: 4rem;"">
    Test message...
</div>";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
