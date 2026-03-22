namespace D20Tek.BlazorComponents.UnitTests.Markdown;

[TestClass]
public class CodeBlockPostProcessorTests
{
    [TestMethod]
    public void AddCopyButtons_WithCodeBlock_WrapsInContainer()
    {
        // arrange
        var html = "<pre><code>var x = 1;</code></pre>";

        // act
        var result = CodeBlockPostProcessor.AddCopyButtons(html);

        // assert
        Assert.Contains("""<div class="markdown-code-block">""", result);
        Assert.Contains("markdown-copy-btn", result);
        Assert.Contains("<pre><code>", result);
        Assert.Contains("</code></pre></div>", result);
    }

    [TestMethod]
    public void AddCopyButtons_WithLanguageClass_WrapsInContainer()
    {
        // arrange
        var html = """<pre><code class="language-csharp">builder.Services.AddMarkdownRenderer();</code></pre>""";

        // act
        var result = CodeBlockPostProcessor.AddCopyButtons(html);

        // assert
        Assert.Contains("""<div class="markdown-code-block">""", result);
        Assert.Contains("markdown-copy-btn", result);
        Assert.Contains("""<pre><code class="language-csharp">""", result);
        Assert.Contains("</code></pre></div>", result);
    }

    [TestMethod]
    public void AddCopyButtons_NoCodeBlocks_ReturnsUnchanged()
    {
        // arrange
        var html = "<p>Some <strong>bold</strong> text and <code>inline code</code>.</p>";

        // act
        var result = CodeBlockPostProcessor.AddCopyButtons(html);

        // assert
        Assert.AreEqual(html, result);
    }

    [TestMethod]
    public void AddCopyButtons_MultipleCodeBlocks_WrapsEach()
    {
        // arrange
        var html = """
            <pre><code class="language-csharp">var x = 1;</code></pre>
            <p>Some text.</p>
            <pre><code class="language-razor">&lt;MarkdownView /&gt;</code></pre>
            """;

        // act
        var result = CodeBlockPostProcessor.AddCopyButtons(html);

        // assert
        var divCount = CountOccurrences(result, """<div class="markdown-code-block">""");
        var closingCount = CountOccurrences(result, "</code></pre></div>");
        Assert.AreEqual(2, divCount);
        Assert.AreEqual(2, closingCount);
    }

    [TestMethod]
    public void AddCopyButtons_EmptyString_ReturnsEmpty()
    {
        // arrange + act
        var result = CodeBlockPostProcessor.AddCopyButtons(string.Empty);

        // assert
        Assert.AreEqual(string.Empty, result);
    }

    private static int CountOccurrences(string source, string target)
    {
        var count = 0;
        var index = 0;
        while ((index = source.IndexOf(target, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += target.Length;
        }
        return count;
    }
}
