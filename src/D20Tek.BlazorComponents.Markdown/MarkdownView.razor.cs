using Microsoft.JSInterop;

namespace D20Tek.BlazorComponents;

public partial class MarkdownView : BaseComponent, IAsyncDisposable
{
    private const string JsModulePath = "./_content/D20Tek.BlazorComponents.Markdown/markdown-copy.js";

    private string _html = string.Empty;
    private IJSObjectReference? _jsModule;

    public MarkdownView() => Size = Size.None;

    [Parameter]
    public string Markdown { get; set; } = string.Empty;

    [Parameter]
    public bool ShowCopyButton { get; set; } = true;

    [Inject]
    private IMarkdownRenderer Renderer { get; set; } = default!;

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = default!;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _html = Renderer.ToHtml(Markdown);
        if (ShowCopyButton)
        {
            _html = CodeBlockPostProcessor.AddCopyButtons(_html);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && ShowCopyButton)
        {
            _jsModule = await JsRuntime.InvokeAsync<IJSObjectReference>("import", JsModulePath);
            await _jsModule.InvokeVoidAsync("init");
        }
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        if (_jsModule is not null)
        {
            await _jsModule.InvokeVoidAsync("dispose");
            await _jsModule.DisposeAsync();
        }
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
