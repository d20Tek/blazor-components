using D20Tek.BlazorComponents;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class MessageBoxPage
{
    private string _lastResult = "None";

    private async Task ShowInformation() =>
        await MessageBox.ShowAsync("This is an informational message.", "Information", MessageType.Information);

    private async Task ShowSuccess() =>
        await MessageBox.ShowSuccessAsync("Your changes have been saved successfully!");

    private async Task ShowWarning() =>
        await MessageBox.ShowWarningAsync("Please review the warnings before continuing.", "Warning");

    private async Task ShowError() =>
        await MessageBox.ShowErrorAsync("An error occurred while processing your request.", "Error");

    private async Task ShowQuestion() =>
        await MessageBox.ShowAsync("Did you know you can use MessageBox for questions?", "Question", MessageType.Question);

    private async Task ConfirmDelete()
    {
        var result = await MessageBox.ConfirmAsync(
            "Are you sure you want to delete this item? This action cannot be undone.",
            "Confirm Delete",
            MessageType.Warning,
            MessageBoxButtons.YesNo);

        _lastResult = result.ToString();

        if (result == MessageBoxResult.Yes)
        {
            await MessageBox.ShowSuccessAsync("Item deleted successfully.");
        }
    }

    private async Task ConfirmSave()
    {
        var result = await MessageBox.ConfirmAsync(
            "Do you want to save changes before closing?",
            "Unsaved Changes",
            MessageType.Question,
            MessageBoxButtons.YesNoCancel);

        _lastResult = result.ToString();

        switch (result)
        {
            case MessageBoxResult.Yes:
                await MessageBox.ShowSuccessAsync("Changes saved successfully.");
                break;
            case MessageBoxResult.No:
                await MessageBox.ShowAsync("Changes discarded.", "Discarded", MessageType.Information);
                break;
            case MessageBoxResult.Cancel:
                _lastResult = "Cancelled - staying on page";
                break;
        }
    }

    private async Task ConfirmAction()
    {
        var result = await MessageBox.ConfirmAsync(
            "Are you sure you want to proceed with this action?",
            "Confirm",
            MessageType.Question,
            MessageBoxButtons.OkCancel);

        _lastResult = result.ToString();

        if (result == MessageBoxResult.Ok)
        {
            await MessageBox.ShowAsync("Action completed.", "Success", MessageType.Success);
        }
    }

    private async Task ShowCustomTitle() =>
        await MessageBox.ShowAsync(
            "This message box has a custom title that describes the context.",
            "Custom Title Example",
            MessageType.Information);

    private async Task ShowLongMessage()
    {
        var longMessage = "This is a longer message that demonstrates how the MessageBox component " +
                         "handles multi-line text content. The dialog will automatically adjust its " +
                         "layout to accommodate longer messages while maintaining readability and " +
                         "proper visual hierarchy.";

        await MessageBox.ShowAsync(longMessage, "Long Message Example", MessageType.Information);
    }

    private async Task ShowSequentialDialogs()
    {
        await MessageBox.ShowAsync("This is the first message.", "Step 1", MessageType.Information);
        await MessageBox.ShowAsync("This is the second message.", "Step 2", MessageType.Success);
        await MessageBox.ShowAsync("This is the final message.", "Step 3", MessageType.Success);
    }

    private async Task SimulateError()
    {
        await MessageBox.ShowErrorAsync(
            "Unable to connect to the server. Please check your internet connection and try again.",
            "Connection Error");

        var retry = await MessageBox.ConfirmAsync(
            "Would you like to retry the connection?",
            "Retry?",
            MessageType.Question,
            MessageBoxButtons.YesNo);

        _lastResult = retry == MessageBoxResult.Yes ? "Retrying..." : "Cancelled";
    }

    private async Task UnsavedChanges()
    {
        var result = await MessageBox.ConfirmAsync(
            "You have unsaved changes. What would you like to do?",
            "Unsaved Changes",
            MessageType.Warning,
            MessageBoxButtons.YesNoCancel);

        _lastResult = result switch
        {
            MessageBoxResult.Yes => "Saved and closed",
            MessageBoxResult.No => "Closed without saving",
            MessageBoxResult.Cancel => "Stayed on page",
            _ => "None"
        };

        if (result == MessageBoxResult.Yes)
        {
            await MessageBox.ShowSuccessAsync("Your changes have been saved.");
        }
    }

    private async Task CompleteOperation()
    {
        await MessageBox.ShowAsync("Processing your request...", "Please Wait", MessageType.Information);
        await Task.Delay(500);
        await MessageBox.ShowSuccessAsync("Operation completed successfully!", "Success");
    }

    private async Task ShowAtTop() =>
        await MessageBox.ShowAsync(
            "This message box is positioned at the top of the screen.",
            "Top Position",
            MessageType.Information,
            VerticalPosition.Top);

    private async Task ShowAtCenter() =>
        await MessageBox.ShowAsync(
            "This message box is centered vertically on the screen.",
            "Center Position",
            MessageType.Information,
            VerticalPosition.Center);

    private async Task ShowAtBottom() =>
        await MessageBox.ShowAsync(
            "This message box is positioned at the bottom of the screen.",
            "Bottom Position",
            MessageType.Information,
            VerticalPosition.Bottom);
}
