using BlazorComponents = D20Tek.BlazorComponents;

namespace D20Tek.BlazorComponents.UnitTests.TogglePanel;

[TestClass]
public class TogglePanelSizeTests
{
    [TestMethod]
    public void Render_DefaultSize_IsMedium()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header"));

        // assert
        var details = comp.Find("details");
        Assert.IsTrue(details.ClassList.Contains("toggle-panel-md"));
        Assert.AreEqual(BlazorComponents.Size.Medium, comp.Instance.Size);
    }

    [TestMethod]
    public void Render_WithSizeNone()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.None));

        // assert
        var details = comp.Find("details");
        Assert.IsTrue(details.ClassList.Contains("toggle-panel"));
        Assert.IsFalse(details.ClassList.Contains("toggle-panel-md"));
    }

    [TestMethod]
    public void Render_WithSizeExtraSmall()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.ExtraSmall));

        // assert
        Assert.IsTrue(comp.Find("details").ClassList.Contains("toggle-panel-xs"));
    }

    [TestMethod]
    public void Render_WithSizeSmall()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.Small));

        // assert
        Assert.IsTrue(comp.Find("details").ClassList.Contains("toggle-panel-sm"));
    }

    [TestMethod]
    public void Render_WithSizeMedium()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.Medium));

        // assert
        Assert.IsTrue(comp.Find("details").ClassList.Contains("toggle-panel-md"));
    }

    [TestMethod]
    public void Render_WithSizeLarge()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.Large));

        // assert
        Assert.IsTrue(comp.Find("details").ClassList.Contains("toggle-panel-lg"));
    }

    [TestMethod]
    public void Render_WithSizeExtraLarge()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.Size, BlazorComponents.Size.ExtraLarge));

        // assert
        Assert.IsTrue(comp.Find("details").ClassList.Contains("toggle-panel-xl"));
    }
}
