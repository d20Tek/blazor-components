namespace D20Tek.BlazorComponents.UnitTests.Markdown;

public partial class MarkdownViewTests
{
    internal static class Expected
    {
        public const string DefaultEmpty = @"<div class=""markdown-view""></div>";

        public const string Heading1 =
            @"<div class=""markdown-view""><h1>Hello World</h1></div>";

        public const string BoldText =
            @"<div class=""markdown-view""><p><strong>bold</strong></p></div>";

        public const string WithCssClass =
            @"<div class=""markdown-view test-class""></div>";

        public const string Multiline =
            @"<div class=""markdown-view""><h2>Title</h2><p>Paragraph text.</p></div>";
    }
}
