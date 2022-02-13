//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System.Collections.Generic;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [mst.TestClass]
    public class SpinnerLabelTests
    {
        [mst.TestMethod]
        public void Render_WithLabel()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label"));

            // assert
            var expectedHtml =
            @"
              <div class=""spinner-grid-container"">
                  <div role=""status"" class=""spinner spinner-area-main""></div>
                  <div class=""spinner-label spinner-label-bottom"">Test label</div>
              </div>
            ";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithLabelPlacement_Top()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label")
                .Add(p => p.LabelPlacement, Placement.Top));

            // assert
            var expectedHtml =
            @"
              <div class=""spinner-grid-container"">
                  <div role=""status"" class=""spinner spinner-area-main""></div>
                  <div class=""spinner-label spinner-label-top"">Test label</div>
              </div>
            ";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithLabelPlacement_Bottom()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label")
                .Add(p => p.LabelPlacement, Placement.Bottom));

            // assert
            var expectedHtml =
            @"
              <div class=""spinner-grid-container"">
                  <div role=""status"" class=""spinner spinner-area-main""></div>
                  <div class=""spinner-label spinner-label-bottom"">Test label</div>
              </div>
            ";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithLabelPlacement_Left()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label")
                .Add(p => p.LabelPlacement, Placement.Left));

            // assert
            var expectedHtml =
            @"
              <div class=""spinner-grid-container"">
                  <div role=""status"" class=""spinner spinner-area-main""></div>
                  <div class=""spinner-label spinner-label-left"">Test label</div>
              </div>
            ";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithLabelPlacement_Right()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Label, "Test label")
                .Add(p => p.LabelPlacement, Placement.Right));

            // assert
            var expectedHtml =
            @"
              <div class=""spinner-grid-container"">
                  <div role=""status"" class=""spinner spinner-area-main""></div>
                  <div class=""spinner-label spinner-label-right"">Test label</div>
              </div>
            ";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
