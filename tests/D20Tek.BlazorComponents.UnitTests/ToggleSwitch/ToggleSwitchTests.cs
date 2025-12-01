using Bunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests.ToggleSwitch
{
    [TestClass]
    public class ToggleSwitchTests
    {
        [TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>();

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id:ignore checked="""">
  <label class=""form-check-label"" for:ignore></label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }

        [TestMethod]
        public void Render_CheckedFalse()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.Checked, false)
                                        .Add(p => p.Label, "Test"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id:ignore>
  <label class=""form-check-label"" for:ignore>Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_BackgroundColor()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.ToggleColor, "darkgreen")
                                        .Add(p => p.Label, "Dark Green"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id:ignore
         style=""background-color: darkgreen"" checked="""">
  <label class=""form-check-label"" for:ignore>Dark Green</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_NonDefaultSize()
        {
            // arrange
            var ctx = new BunitContext();

            // act
            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.Size, Size.Large)
                                        .Add(p => p.Label, "Large Test"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch toggle-lg"">
  <input class=""form-check-input"" type=""checkbox"" id:ignore checked="""">
  <label class=""form-check-label"" for:ignore>Large Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void CheckChanged()
        {
            // arrange
            var ctx = new BunitContext();
            bool isChecked = true;

            var comp = ctx.Render<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.CheckedChanged, (arg) => { isChecked = arg; })
                                        .Add(p => p.Label, "Change Test"));

            // act
            comp.Find("input").Change<bool>(false);

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id:ignore>
  <label class=""form-check-label"" for:ignore>Change Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
            Assert.IsFalse(isChecked);
        }
    }
}
