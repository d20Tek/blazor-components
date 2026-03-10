namespace D20Tek.BlazorComponents;

public interface IMessageBoxService
{
    Task ShowAsync(
        string message,
        string title = Constants.MessageTextDefault,
        MessageType type = MessageType.Information);

    Task<MessageBoxResult> ConfirmAsync(
        string message,
        string title = Constants.ConfirmTextDefault,
        MessageType type = MessageType.Question,
        MessageBoxButtons buttons = MessageBoxButtons.YesNo);

    Task ShowErrorAsync(string message, string title = Constants.ErrorTextDefault);

    Task ShowWarningAsync(string message, string title = Constants.WarningTextDefault);

    Task ShowSuccessAsync(string message, string title = Constants.SuccessTextDefault);

    event Action<MessageBoxOptions>? OnShow;

    void SetResult(MessageBoxResult result);
}
