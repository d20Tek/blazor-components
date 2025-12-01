using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [TestClass]
    public class SpinnerLabelTests
    {
        [TestMethod]
        public void Render_WithLabel()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters.Add(p => p.Label, "Test label"));

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

        [TestMethod]
        public void Render_WithLabelPlacement_Top()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters
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

        [TestMethod]
        public void Render_WithLabelPlacement_Bottom()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters
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

        [TestMethod]
        public void Render_WithLabelPlacement_Left()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters
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

        [TestMethod]
        public void Render_WithLabelPlacement_Right()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<Spinner>(parameters => parameters
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
