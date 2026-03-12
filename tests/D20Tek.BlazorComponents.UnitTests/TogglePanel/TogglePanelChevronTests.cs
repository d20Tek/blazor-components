using BlazorComponents = D20Tek.BlazorComponents;

namespace D20Tek.BlazorComponents.UnitTests.TogglePanel;

[TestClass]
public class TogglePanelChevronTests
{
    [TestMethod]
    public void Render_DefaultShowsChevron()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header"));

        // assert
        var chevron = comp.Find(".toggle-panel__chevron");
        Assert.IsNotNull(chevron);
        Assert.IsTrue(comp.Instance.ShowChevron);
    }

    [TestMethod]
    public void Render_WithShowChevronFalse_HidesChevron()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.ShowChevron, false));

        // assert
        var chevrons = comp.FindAll(".toggle-panel__chevron");
        Assert.AreEqual(0, chevrons.Count);
    }

    [TestMethod]
    public void Render_Chevron_HasAriaHidden()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header"));

        // assert
        var chevron = comp.Find(".toggle-panel__chevron");
        Assert.AreEqual("true", chevron.GetAttribute("aria-hidden"));
    }

    [TestMethod]
    public void Render_Chevron_ContainsSvg()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header"));

        // assert
        var svg = comp.Find(".toggle-panel__chevron svg");
        Assert.IsNotNull(svg);
    }

    [TestMethod]
    public void Render_Chevron_ShowChevronTrue_DefaultValue()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.ShowChevron, true));

        // assert
        var chevron = comp.Find(".toggle-panel__chevron");
        Assert.IsNotNull(chevron);
        Assert.AreEqual(true, comp.Instance.ShowChevron);
    }
}
