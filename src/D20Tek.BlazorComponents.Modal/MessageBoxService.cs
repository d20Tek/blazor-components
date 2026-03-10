namespace D20Tek.BlazorComponents;

public sealed class MessageBoxService : IMessageBoxService
{
    private TaskCompletionSource<MessageBoxResult>? _currentTaskCompletionSource;

    public event Action<MessageBoxOptions>? OnShow;

    public Task ShowAsync(
        string message,
        string title = Constants.MessageTextDefault,
        MessageType type = MessageType.Information,
        VerticalPosition position = VerticalPosition.Top) =>
        ShowMessageBoxAsync(message, title, type, MessageBoxButtons.Ok, position);

    public Task<MessageBoxResult> ConfirmAsync(
        string message,
        string title = Constants.ConfirmTextDefault,
        MessageType type = MessageType.Question,
        MessageBoxButtons buttons = MessageBoxButtons.YesNo,
        VerticalPosition position = VerticalPosition.Top) =>
        ShowMessageBoxAsync(message, title, type, buttons, position);

    public Task ShowErrorAsync(
        string message,
        string title = Constants.ErrorTextDefault,
        VerticalPosition position = VerticalPosition.Top) =>
        ShowMessageBoxAsync(message, title, MessageType.Error, MessageBoxButtons.Ok, position);

    public Task ShowWarningAsync(
        string message,
        string title = Constants.WarningTextDefault,
        VerticalPosition position = VerticalPosition.Top) =>
        ShowMessageBoxAsync(message, title, MessageType.Warning, MessageBoxButtons.Ok, position);

    public Task ShowSuccessAsync(
        string message,
        string title = Constants.SuccessTextDefault,
        VerticalPosition position = VerticalPosition.Top) =>
        ShowMessageBoxAsync(message, title, MessageType.Success, MessageBoxButtons.Ok, position);

    public void SetResult(MessageBoxResult result)
    {
        _currentTaskCompletionSource?.TrySetResult(result);
        _currentTaskCompletionSource = null;
    }

    private Task<MessageBoxResult> ShowMessageBoxAsync(
        string message,
        string title,
        MessageType type,
        MessageBoxButtons buttons,
        VerticalPosition position)
    {
        var tcs = new TaskCompletionSource<MessageBoxResult>();
        _currentTaskCompletionSource = tcs;

        var options = new MessageBoxOptions
        {
            Title = title,
            Message = message,
            Type = type,
            Buttons = buttons,
            Position = position,
            TaskCompletionSource = tcs
        };

        OnShow?.Invoke(options);

        return tcs.Task;
    }
}
