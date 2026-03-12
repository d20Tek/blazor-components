using Microsoft.AspNetCore.Components;

namespace D20Tek.BlazorComponents.UnitTests.TogglePanel;

[TestClass]
public class TogglePanelTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>();

        // assert
        var details = comp.Find("details");
        Assert.IsNotNull(details);
        Assert.IsTrue(details.ClassList.Contains("toggle-panel"));
        Assert.IsTrue(details.ClassList.Contains("toggle-panel-md"));
        Assert.IsNull(details.GetAttribute("open"));
    }

    [TestMethod]
    public void Render_IsVisibleFalse()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.IsVisible, false));

        // assert
        comp.MarkupMatches(string.Empty);
    }

    [TestMethod]
    public void Render_WithSummary()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Test Header"));

        // assert
        var summaryText = comp.Find(".toggle-panel__summary-text");
        Assert.AreEqual("Test Header", summaryText.TextContent.Trim());
        Assert.AreEqual("Test Header", comp.Instance.Summary);
    }

    [TestMethod]
    public void Render_WithChildContent()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .Add(p => p.ChildContent, (RenderFragment)(b => b.AddMarkupContent(0, "<p>Body content</p>"))));

        // assert
        var body = comp.Find(".toggle-panel__body");
        Assert.IsNotNull(body);
        Assert.Contains("Body content", body.InnerHtml);
    }

    [TestMethod]
    public void Render_WithSummaryContent()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.SummaryContent,
                (RenderFragment)(b => b.AddMarkupContent(0, "<span class=\"custom\">Rich Header</span>"))));

        // assert
        var custom = comp.Find(".custom");
        Assert.AreEqual("Rich Header", custom.TextContent);
    }

    [TestMethod]
    public void Render_WithRemainingAttributes()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<BlazorComponents.TogglePanel>(parameters =>
            parameters.Add(p => p.Summary, "Header")
                      .AddUnmatched("data-testid", "my-panel"));

        // assert
        var details = comp.Find("details");
        Assert.AreEqual("my-panel", details.GetAttribute("data-testid"));
    }
}
