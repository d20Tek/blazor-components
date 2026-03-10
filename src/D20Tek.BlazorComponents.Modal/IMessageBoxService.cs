namespace D20Tek.BlazorComponents;

public interface IMessageBoxService
{
    Task ShowAsync(
        string message,
        string title = Constants.MessageTextDefault,
        MessageType type = MessageType.Information,
        VerticalPosition position = VerticalPosition.Top);

    Task<MessageBoxResult> ConfirmAsync(
        string message,
        string title = Constants.ConfirmTextDefault,
        MessageType type = MessageType.Question,
        MessageBoxButtons buttons = MessageBoxButtons.YesNo,
        VerticalPosition position = VerticalPosition.Top);

    Task ShowErrorAsync(
        string message,
        string title = Constants.ErrorTextDefault,
        VerticalPosition position = VerticalPosition.Top);

    Task ShowWarningAsync(
        string message,
        string title = Constants.WarningTextDefault,
        VerticalPosition position = VerticalPosition.Top);

    Task ShowSuccessAsync(
        string message,
        string title = Constants.SuccessTextDefault,
        VerticalPosition position = VerticalPosition.Top);

    event Action<MessageBoxOptions>? OnShow;

    void SetResult(MessageBoxResult result);
}
