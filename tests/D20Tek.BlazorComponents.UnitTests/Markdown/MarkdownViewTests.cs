using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.BlazorComponents.UnitTests.Markdown;

[TestClass]
public partial class MarkdownViewTests
{
    private static BunitContext CreateContext(string renderedHtml = "")
    {
        var ctx = new BunitContext();
        ctx.Services.AddSingleton<IMarkdownRenderer>(new FakeMarkdownRenderer(renderedHtml));
        return ctx;
    }

    [TestMethod]
    public void DefaultRender_EmptyMarkdown()
    {
        // arrange
        var ctx = CreateContext();

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>();

        // assert
        comp.MarkupMatches(Expected.DefaultEmpty);
    }

    [TestMethod]
    public void Render_IsVisibleFalse()
    {
        // arrange
        var ctx = CreateContext();

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.IsVisible, false));

        // assert
        comp.MarkupMatches(string.Empty);
    }

    [TestMethod]
    public void Render_Heading1()
    {
        // arrange
        var ctx = CreateContext("<h1>Hello World</h1>");

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, "# Hello World"));

        // assert
        comp.MarkupMatches(Expected.Heading1);
    }

    [TestMethod]
    public void Render_BoldText()
    {
        // arrange
        var ctx = CreateContext("<p><strong>bold</strong></p>");

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, "**bold**"));

        // assert
        comp.MarkupMatches(Expected.BoldText);
    }

    [TestMethod]
    public void Render_WithAttributeSplat()
    {
        // arrange
        var ctx = CreateContext();
        var attr = new Dictionary<string, object>
        {
            { "class", "test-class" }
        };

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.RemainingAttributes, attr));

        // assert
        comp.MarkupMatches(Expected.WithCssClass);
    }

    [TestMethod]
    public void Render_NullMarkdown()
    {
        // arrange
        var ctx = CreateContext();

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, null!));

        // assert
        comp.MarkupMatches(Expected.DefaultEmpty);
    }

    [TestMethod]
    public void Render_MultilineMarkdown()
    {
        // arrange
        var ctx = CreateContext("<h2>Title</h2><p>Paragraph text.</p>");

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, "## Title\n\nParagraph text."));

        // assert
        comp.MarkupMatches(Expected.Multiline);
    }

    [TestMethod]
    public void Renderer_ToHtml_CalledOnParametersSet()
    {
        // arrange
        var ctx = new BunitContext();
        var trackingRenderer = new TrackingMarkdownRenderer("<p>result</p>");
        ctx.Services.AddSingleton<IMarkdownRenderer>(trackingRenderer);

        // act
        ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, "test-markdown"));

        // assert
        Assert.AreEqual(1, trackingRenderer.CallCount);
        Assert.AreEqual("test-markdown", trackingRenderer.LastInput);
    }

    [TestMethod]
    public void Render_WithRealMarkdigRenderer_ConvertsMarkdownToHtml()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.Services.AddSingleton<IMarkdownRenderer>(new MarkdigMarkdownRenderer());

        // act
        var comp = ctx.Render<BlazorComponents.MarkdownView>(parameters =>
            parameters.Add(p => p.Markdown, "# Hello\n\n**bold** and _italic_"));

        // assert - heading gets id attribute from UseAutoIdentifiers (part of UseAdvancedExtensions)
        var heading = comp.Find("h1");
        Assert.AreEqual("Hello", heading.TextContent);
        Assert.AreEqual("hello", heading.GetAttribute("id"));

        Assert.AreEqual("bold", comp.Find("strong").TextContent);
        Assert.AreEqual("italic", comp.Find("em").TextContent);
    }
}

internal class FakeMarkdownRenderer(string output) : IMarkdownRenderer
{
    private readonly string _output = output;

    public string ToHtml(string markdown) => _output;
}

internal class TrackingMarkdownRenderer(string output) : IMarkdownRenderer
{
    private readonly string _output = output;

    public int CallCount { get; private set; }

    public string? LastInput { get; private set; }

    public string ToHtml(string markdown)
    {
        CallCount++;
        LastInput = markdown;
        return _output;
    }
}
