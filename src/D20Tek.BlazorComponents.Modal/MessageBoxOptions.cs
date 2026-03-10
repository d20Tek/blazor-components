namespace D20Tek.BlazorComponents;

public class MessageBoxOptions
{
    public string Title { get; set; } = Constants.MessageTextDefault;

    public string Message { get; set; } = string.Empty;

    public MessageType Type { get; set; } = MessageType.Information;

    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.Ok;

    public VerticalPosition Position { get; set; } = VerticalPosition.Top;

    public TaskCompletionSource<MessageBoxResult> TaskCompletionSource { get; set; } = default!;
}
