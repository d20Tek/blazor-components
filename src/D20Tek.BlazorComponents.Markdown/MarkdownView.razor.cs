namespace D20Tek.BlazorComponents;

public partial class MarkdownView : BaseComponent
{
    private string _html = string.Empty;

    public MarkdownView() => Size = Size.None;

    [Parameter]
    public string Markdown { get; set; } = string.Empty;

    [Inject]
    private IMarkdownRenderer Renderer { get; set; } = default!;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _html = Renderer.ToHtml(Markdown);
    }

    protected override string? CalculateCssClasses() =>
        new CssBuilder("markdown-view")
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
