using Markdig;

namespace D20Tek.BlazorComponents;

public class MarkdigRenderer : IMarkdownRenderer
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdigRenderer()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    public string ToHtml(string markdown) => Markdown.ToHtml(markdown, _pipeline);
}
