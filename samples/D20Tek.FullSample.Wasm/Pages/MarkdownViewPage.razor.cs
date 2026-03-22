namespace D20Tek.FullSample.Wasm.Pages;

public partial class MarkdownViewPage
{
    private string _liveMarkdown = "## Live Preview\n\nType **Markdown** here and see it _rendered_ in real time.\n\n- Item one\n- Item two\n- Item three\n\n```csharp\nvar greeting = \"Hello, World!\";\nConsole.WriteLine(greeting);\n```";

    private readonly string _sampleMarkdown = """
        # Welcome to MarkdownView

        This component renders **Markdown** text as HTML using the [Markdig](https://github.com/xoofx/markdig) library.

        ## Features

        - Renders _all standard_ Markdown syntax
        - Supports **bold**, *italic*, and ~~strikethrough~~ text
        - Auto-links like https://github.com/d20Tek are detected
        - Integrates via `IMarkdownRenderer` dependency injection

        > **Tip:** Register the renderer once with `builder.Services.AddMarkdownRenderer()` and inject it anywhere.

        ---

        Paragraph text with `inline code` and a [hyperlink](https://github.com/d20Tek).
        """;

    private readonly string _tableMarkdown = """
        | Component      | Description                        | Status       |
        |----------------|------------------------------------|--------------|
        | Spinner        | Loading indicator overlays         | &#x2713; Done |
        | Timer          | Countdown and elapsed timers       | &#x2713; Done |
        | Modal Dialog   | Native `<dialog>` based modals     | &#x2713; Done |
        | Toggle Switch  | Boolean toggle control             | &#x2713; Done |
        | Markdown View  | Markdown-to-HTML renderer          | In progress |
        """;

    private readonly string _taskMarkdown = """
        ### Release Checklist

        - [x] Create component library
        - [x] Write unit tests with bUnit
        - [x] Register IMarkdownRenderer with DI
        - [x] Add sample page to demo app
        - [ ] Publish NuGet package
        - [ ] Write documentation
        """;

    private readonly string _codeMarkdown = """
        ### Usage Example

        Register the renderer in `Program.cs`:

        ```csharp
        builder.Services.AddMarkdownRenderer();
        ```

        Then use the component in any `.razor` file:

        ```razor
        <MarkdownView Markdown="@myMarkdownString" />
        ```
        """;
}
