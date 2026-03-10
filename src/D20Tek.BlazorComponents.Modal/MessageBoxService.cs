namespace D20Tek.BlazorComponents;

public sealed class MessageBoxService : IMessageBoxService
{
    private TaskCompletionSource<MessageBoxResult>? _currentTaskCompletionSource;

    public event Action<MessageBoxOptions>? OnShow;

    public Task ShowAsync(
        string message,
        string title = Constants.MessageTextDefault,
        MessageType type = MessageType.Information) => ShowMessageBoxAsync(message, title, type, MessageBoxButtons.Ok);

    public Task<MessageBoxResult> ConfirmAsync(
        string message,
        string title = Constants.ConfirmTextDefault,
        MessageType type = MessageType.Question,
        MessageBoxButtons buttons = MessageBoxButtons.YesNo) =>
        ShowMessageBoxAsync(message, title, type, buttons);

    public Task ShowErrorAsync(
        string message,
        string title = Constants.ErrorTextDefault) =>
        ShowMessageBoxAsync(message, title, MessageType.Error, MessageBoxButtons.Ok);

    public Task ShowWarningAsync(string message, string title = Constants.WarningTextDefault) =>
        ShowMessageBoxAsync(message, title, MessageType.Warning, MessageBoxButtons.Ok);

    public Task ShowSuccessAsync(string message, string title = Constants.SuccessTextDefault) =>
        ShowMessageBoxAsync(message, title, MessageType.Success, MessageBoxButtons.Ok);

    public void SetResult(MessageBoxResult result)
    {
        _currentTaskCompletionSource?.TrySetResult(result);
        _currentTaskCompletionSource = null;
    }

    private Task<MessageBoxResult> ShowMessageBoxAsync(
        string message,
        string title,
        MessageType type,
        MessageBoxButtons buttons)
    {
        var tcs = new TaskCompletionSource<MessageBoxResult>();
        _currentTaskCompletionSource = tcs;

        var options = new MessageBoxOptions
        {
            Title = title,
            Message = message,
            Type = type,
            Buttons = buttons,
            TaskCompletionSource = tcs
        };

        OnShow?.Invoke(options);

        return tcs.Task;
    }
}
