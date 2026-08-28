namespace D20Tek.BlazorComponents;

internal static class ResultToastFragmentBuilder
{
    public static RenderFragment BuildTextFragment(string message) => 
        builder => builder.AddContent(0, message);

    public static RenderFragment BuildFailureFragment(string header, IReadOnlyList<string> messages) => 
        builder =>
        {
            var seq = 0;
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "result-toast");

            if (!string.IsNullOrEmpty(header))
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "result-toast__header");
                builder.AddContent(seq++, header);
                builder.CloseElement();
            }

            if (messages.Count > 0)
            {
                builder.OpenElement(seq++, "ul");
                builder.AddAttribute(seq++, "class", "result-toast__errors");
                foreach (var message in messages)
                {
                    builder.OpenElement(seq++, "li");
                    builder.AddContent(seq++, message);
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        };
}
