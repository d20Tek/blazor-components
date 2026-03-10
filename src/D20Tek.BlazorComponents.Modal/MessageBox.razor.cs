namespace D20Tek.BlazorComponents;

public partial class MessageBox : MessageBoxBase
{
    [Parameter, EditorRequired]
    public string Message { get; set; } = string.Empty;

    [Parameter]
    public MessageType Type { get; set; } = MessageType.Information;

    [Parameter]
    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.Ok;

    [Parameter]
    public EventCallback<MessageBoxResult> OnResult { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ShowAsync();
        }
    }

    private async Task HandleButtonClick(MessageBoxResult result)
    {
        await CloseAsync();
        await OnResult.InvokeAsync(result);
    }

    private Task HandleCloseClick() => HandleButtonClick(MessageBoxResult.None);

    private RenderFragment GetIcon() => Type switch
    {
        MessageType.Success => builder => builder.AddMarkupContent(0, SuccessIcon),
        MessageType.Warning => builder => builder.AddMarkupContent(0, WarningIcon),
        MessageType.Error => builder => builder.AddMarkupContent(0, ErrorIcon),
        MessageType.Question => builder => builder.AddMarkupContent(0, QuestionIcon),
        _ => builder => builder.AddMarkupContent(0, InformationIcon)
    };

    private RenderFragment GetButtons() => Buttons switch
    {
        MessageBoxButtons.OkCancel => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", "modal-dialog__btn modal-dialog__btn-cancel");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Cancel)));
            builder.AddContent(4, "Cancel");
            builder.CloseElement();

            builder.OpenElement(10, "button");
            builder.AddAttribute(11, "type", "button");
            builder.AddAttribute(12, "class", "modal-dialog__btn modal-dialog__btn-submit");
            builder.AddAttribute(13, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Ok)));
            builder.AddContent(14, "OK");
            builder.CloseElement();
        },
        MessageBoxButtons.YesNo => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", "modal-dialog__btn modal-dialog__btn-cancel");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.No)));
            builder.AddContent(4, "No");
            builder.CloseElement();

            builder.OpenElement(10, "button");
            builder.AddAttribute(11, "type", "button");
            builder.AddAttribute(12, "class", "modal-dialog__btn modal-dialog__btn-submit");
            builder.AddAttribute(13, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Yes)));
            builder.AddContent(14, "Yes");
            builder.CloseElement();
        },
        MessageBoxButtons.YesNoCancel => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", "modal-dialog__btn modal-dialog__btn-cancel");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Cancel)));
            builder.AddContent(4, "Cancel");
            builder.CloseElement();

            builder.OpenElement(10, "button");
            builder.AddAttribute(11, "type", "button");
            builder.AddAttribute(12, "class", "modal-dialog__btn modal-dialog__btn-secondary");
            builder.AddAttribute(13, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.No)));
            builder.AddContent(14, "No");
            builder.CloseElement();

            builder.OpenElement(20, "button");
            builder.AddAttribute(21, "type", "button");
            builder.AddAttribute(22, "class", "modal-dialog__btn modal-dialog__btn-submit");
            builder.AddAttribute(23, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Yes)));
            builder.AddContent(24, "Yes");
            builder.CloseElement();
        },
        _ => builder =>
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", "modal-dialog__btn modal-dialog__btn-submit");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, () => HandleButtonClick(MessageBoxResult.Ok)));
            builder.AddContent(4, "OK");
            builder.CloseElement();
        }
    };

    private const string InformationIcon = """
        <svg viewBox="0 0 24 24" class="message-box__icon--info">
            <circle cx="12" cy="12" r="10" fill="#3b82f6"/>
            <text x="12" y="16" text-anchor="middle" fill="white" font-size="14" font-weight="bold">i</text>
        </svg>
        """;

    private const string SuccessIcon = """
        <svg viewBox="0 0 24 24" class="message-box__icon--success">
            <circle cx="12" cy="12" r="10" fill="#22c55e"/>
            <path d="M8 12l2 2 4-4" stroke="white" stroke-width="2" fill="none"/>
        </svg>
        """;

    private const string WarningIcon = """
        <svg viewBox="0 0 24 24" class="message-box__icon--warning">
            <path d="M12 2L2 22h20L12 2z" fill="#f59e0b"/>
            <text x="12" y="18" text-anchor="middle" fill="white" font-size="12" font-weight="bold">!</text>
        </svg>
        """;

    private const string ErrorIcon = """
        <svg viewBox="0 0 24 24" class="message-box__icon--error">
            <circle cx="12" cy="12" r="10" fill="#ef4444"/>
            <path d="M8 8l8 8M16 8l-8 8" stroke="white" stroke-width="2"/>
        </svg>
        """;

    private const string QuestionIcon = """
        <svg viewBox="0 0 24 24" class="message-box__icon--question">
            <circle cx="12" cy="12" r="10" fill="#3b82f6"/>
            <text x="12" y="16" text-anchor="middle" fill="white" font-size="12" font-weight="bold">?</text>
        </svg>
        """;
}
