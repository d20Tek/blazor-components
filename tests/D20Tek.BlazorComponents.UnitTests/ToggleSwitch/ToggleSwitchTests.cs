//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
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
            var ctx = new Bunit.TestContext();

            // act
            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>();

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch mt-2 toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id=""toggle-switch"" checked="""">
  <label class=""form-check-label"" for=""toggle-switch""></label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new Bunit.TestContext();

            // act
            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }

        [TestMethod]
        public void Render_CheckedFalse()
        {
            // arrange
            var ctx = new Bunit.TestContext();

            // act
            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.Checked, false)
                                        .Add(p => p.Label, "Test"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch mt-2 toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id=""toggle-switch"">
  <label class=""form-check-label"" for=""toggle-switch"">Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_BackgroundColor()
        {
            // arrange
            var ctx = new Bunit.TestContext();

            // act
            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.ToggleColor, "darkgreen")
                                        .Add(p => p.Label, "Dark Green"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch mt-2 toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id=""toggle-switch""
         style=""background-color: darkgreen"" checked="""">
  <label class=""form-check-label"" for=""toggle-switch"">Dark Green</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void Render_NonDefaultSize()
        {
            // arrange
            var ctx = new Bunit.TestContext();

            // act
            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.Size, Size.Large)
                                        .Add(p => p.Label, "Large Test"));

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch mt-2 toggle-lg"">
  <input class=""form-check-input"" type=""checkbox"" id=""toggle-switch"" checked="""">
  <label class=""form-check-label"" for=""toggle-switch"">Large Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
        }

        [TestMethod]
        public void CheckChanged()
        {
            // arrange
            var ctx = new Bunit.TestContext();
            bool isChecked = true;

            var comp = ctx.RenderComponent<D20Tek.BlazorComponents.ToggleSwitch>(
                parameters => parameters.Add(p => p.CheckedChanged, (arg) => { isChecked = arg; })
                                        .Add(p => p.Label, "Change Test"));

            // act
            comp.Find("#toggle-switch").Change<bool>(false);

            // assert
            var expectedHtml =
@"
<div class=""form-check form-switch mt-2 toggle-md"">
  <input class=""form-check-input"" type=""checkbox"" id=""toggle-switch"">
  <label class=""form-check-label"" for=""toggle-switch"">Change Test</label>
</div>
";
            comp.MarkupMatches(expectedHtml);
            Assert.IsFalse(isChecked);
        }
    }
}
