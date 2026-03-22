using System.Text.RegularExpressions;

namespace D20Tek.BlazorComponents;

internal static partial class CodeBlockPostProcessor
{
    private const string CopyButtonHtml =
        """<button class="markdown-copy-btn" type="button" title="Copy code"><svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg></button>""";

    public static string AddCopyButtons(string html) =>
        PreCodeRegex().Replace(html, $"""<div class="markdown-code-block">{CopyButtonHtml}$0""")
                      .Replace("</code></pre>", "</code></pre></div>");

    [GeneratedRegex(@"<pre><code(?:\s[^>]*)?>")]
    private static partial Regex PreCodeRegex();
}

