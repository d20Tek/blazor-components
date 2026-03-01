# MessageBox Component Implementation Plan

## Overview
Create a `MessageBox` component with a service-based API that allows showing simple alert and confirmation dialogs from anywhere in the application. This follows the familiar Windows/WinForms MessageBox pattern but adapted for Blazor's component model.

## Design Goals
- **Simple API** - `await MessageBox.ShowAsync("message")` style calls
- **Service-based** - Inject `IMessageBoxService` and call methods
- **Built-in icons** - SVG icons for Error, Warning, Info, Success, Question
- **Awaitable results** - `ConfirmAsync` returns the user's choice
- **Minimal setup** - Just add provider to layout and register service
- **Consistent styling** - Reuses ModalDialog CSS classes

## Architecture

```
???????????????????????????????????????????????????????????????
?  MainLayout.razor                                           ?
?  ?????????????????????????????????????????????????????????? ?
?  ?  <MessageBoxProvider />  ? Subscribes to service       ? ?
?  ?         ?                                              ? ?
?  ?         ?                                              ? ?
?  ?  <MessageBox /> (rendered when triggered)              ? ?
?  ?                                                        ? ?
?  ?  @Body                                                 ? ?
?  ?  ????????????????????????????????????????????????????  ? ?
?  ?  ?  AnyPage.razor                                   ?  ? ?
?  ?  ?  @inject IMessageBoxService MessageBox           ?  ? ?
?  ?  ?                                                  ?  ? ?
?  ?  ?  var result = await MessageBox.ConfirmAsync(...) ?  ? ?
?  ?  ?         ?                                        ?  ? ?
?  ?  ????????????????????????????????????????????????????  ? ?
?  ?            ?                                           ? ?
?  ?            ?                                           ? ?
?  ?  MessageBoxService (singleton)                         ? ?
?  ?    - Raises OnShow event                               ? ?
?  ?    - TaskCompletionSource waits for result             ? ?
?  ?    - Provider catches event, shows dialog              ? ?
?  ?    - User clicks button, result returned               ? ?
?  ?????????????????????????????????????????????????????????? ?
???????????????????????????????????????????????????????????????
```

## Enums

### MessageType
```csharp
public enum MessageType
{
    Information = 0,  // Blue "i" icon
    Success = 1,      // Green checkmark icon
    Warning = 2,      // Yellow/orange triangle icon
    Error = 3,        // Red X icon
    Question = 4      // Blue question mark icon
}
```

### MessageBoxButtons
```csharp
public enum MessageBoxButtons
{
    Ok = 0,           // Just "OK" button
    OkCancel = 1,     // "OK" and "Cancel" buttons
    YesNo = 2,        // "Yes" and "No" buttons
    YesNoCancel = 3   // "Yes", "No", and "Cancel" buttons
}
```

### MessageBoxResult
```csharp
public enum MessageBoxResult
{
    None = 0,    // Dialog was closed without selection
    Ok = 1,      // User clicked OK
    Cancel = 2,  // User clicked Cancel
    Yes = 3,     // User clicked Yes
    No = 4       // User clicked No
}
```

## Service Interface

### IMessageBoxService
```csharp
public interface IMessageBoxService
{
    /// <summary>
    /// Shows a simple message dialog with an OK button.
    /// </summary>
    Task ShowAsync(string message, string title = "Message", 
                   MessageType type = MessageType.Information);

    /// <summary>
    /// Shows a confirmation dialog and returns the user's choice.
    /// </summary>
    Task<MessageBoxResult> ConfirmAsync(string message, string title = "Confirm",
                                        MessageType type = MessageType.Question,
                                        MessageBoxButtons buttons = MessageBoxButtons.YesNo);

    /// <summary>
    /// Shows an error message dialog.
    /// </summary>
    Task ShowErrorAsync(string message, string title = "Error");

    /// <summary>
    /// Shows a warning message dialog.
    /// </summary>
    Task ShowWarningAsync(string message, string title = "Warning");

    /// <summary>
    /// Shows a success message dialog.
    /// </summary>
    Task ShowSuccessAsync(string message, string title = "Success");

    /// <summary>
    /// Event raised when a message box should be shown.
    /// </summary>
    event Action<MessageBoxOptions>? OnShow;

    /// <summary>
    /// Called by provider to return the result.
    /// </summary>
    void SetResult(MessageBoxResult result);
}
```

### MessageBoxOptions (internal DTO)
```csharp
public class MessageBoxOptions
{
    public string Title { get; set; } = "Message";
    public string Message { get; set; } = string.Empty;
    public MessageType Type { get; set; } = MessageType.Information;
    public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.Ok;
    public TaskCompletionSource<MessageBoxResult> TaskCompletionSource { get; set; } = default!;
}
```

## Components

### MessageBoxProvider
The provider component that listens to the service and renders the MessageBox.

```razor
@implements IDisposable
@inject IMessageBoxService MessageBoxService

@if (_isVisible)
{
    <MessageBox Title="@_options.Title"
                Message="@_options.Message"
                Type="@_options.Type"
                Buttons="@_options.Buttons"
                OnResult="HandleResult" />
}

@code {
    private bool _isVisible;
    private MessageBoxOptions _options = new();

    protected override void OnInitialized()
    {
        MessageBoxService.OnShow += ShowMessageBox;
    }

    private void ShowMessageBox(MessageBoxOptions options)
    {
        _options = options;
        _isVisible = true;
        InvokeAsync(StateHasChanged);
    }

    private void HandleResult(MessageBoxResult result)
    {
        _isVisible = false;
        MessageBoxService.SetResult(result);
        StateHasChanged();
    }

    public void Dispose()
    {
        MessageBoxService.OnShow -= ShowMessageBox;
    }
}
```

### MessageBox
The visual dialog component.

**Parameters:**
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Title` | `string` | `"Message"` | Dialog title |
| `Message` | `string` | `""` | Message text to display |
| `Type` | `MessageType` | `Information` | Determines icon shown |
| `Buttons` | `MessageBoxButtons` | `Ok` | Which buttons to show |
| `OnResult` | `EventCallback<MessageBoxResult>` | - | Callback when button clicked |

**Structure:**
```
<dialog class="modal-dialog modal-dialog-sm">
    <header class="modal-dialog__header">
        <h2>@Title</h2>
        <button class="close-btn">×</button>
    </header>
    <div class="modal-dialog__body message-box__body">
        <div class="message-box__icon message-box__icon--error">
            <!-- SVG icon based on Type -->
        </div>
        <p class="message-box__message">@Message</p>
    </div>
    <footer class="modal-dialog__footer">
        <!-- Buttons based on Buttons parameter -->
    </footer>
</dialog>
```

## SVG Icons

### Information Icon (Blue circle with "i")
```svg
<svg viewBox="0 0 24 24" class="message-box__icon message-box__icon--info">
    <circle cx="12" cy="12" r="10" fill="#3b82f6"/>
    <text x="12" y="16" text-anchor="middle" fill="white" font-size="14" font-weight="bold">i</text>
</svg>
```

### Success Icon (Green checkmark)
```svg
<svg viewBox="0 0 24 24" class="message-box__icon message-box__icon--success">
    <circle cx="12" cy="12" r="10" fill="#22c55e"/>
    <path d="M8 12l2 2 4-4" stroke="white" stroke-width="2" fill="none"/>
</svg>
```

### Warning Icon (Yellow triangle)
```svg
<svg viewBox="0 0 24 24" class="message-box__icon message-box__icon--warning">
    <path d="M12 2L2 22h20L12 2z" fill="#f59e0b"/>
    <text x="12" y="18" text-anchor="middle" fill="white" font-size="12" font-weight="bold">!</text>
</svg>
```

### Error Icon (Red circle with X)
```svg
<svg viewBox="0 0 24 24" class="message-box__icon message-box__icon--error">
    <circle cx="12" cy="12" r="10" fill="#ef4444"/>
    <path d="M8 8l8 8M16 8l-8 8" stroke="white" stroke-width="2"/>
</svg>
```

### Question Icon (Blue circle with ?)
```svg
<svg viewBox="0 0 24 24" class="message-box__icon message-box__icon--question">
    <circle cx="12" cy="12" r="10" fill="#3b82f6"/>
    <text x="12" y="16" text-anchor="middle" fill="white" font-size="12" font-weight="bold">?</text>
</svg>
```

## CSS Classes

```css
/* Message Box specific styles */
.message-box__body {
    display: flex;
    align-items: flex-start;
    gap: 1rem;
}

.message-box__icon {
    flex-shrink: 0;
    width: 2.5rem;
    height: 2.5rem;
}

.message-box__message {
    flex: 1;
    margin: 0;
    padding-top: 0.25rem;
    line-height: 1.5;
}
```

## Implementation Steps

### Step 1: Create MessageType enum
- Add `MessageType.cs` to Core project
- Values: Information, Success, Warning, Error, Question

### Step 2: Create MessageBoxButtons enum
- Add `MessageBoxButtons.cs` to Core project
- Values: Ok, OkCancel, YesNo, YesNoCancel

### Step 3: Create MessageBoxResult enum
- Add `MessageBoxResult.cs` to Core project
- Values: None, Ok, Cancel, Yes, No

### Step 4: Create MessageBoxOptions class
- Add `MessageBoxOptions.cs` to Modal project
- Properties: Title, Message, Type, Buttons, TaskCompletionSource

### Step 5: Create IMessageBoxService interface
- Add `IMessageBoxService.cs` to Modal project
- Methods: ShowAsync, ConfirmAsync, ShowErrorAsync, ShowWarningAsync, ShowSuccessAsync
- Event: OnShow
- Method: SetResult

### Step 6: Create MessageBoxService implementation
- Add `MessageBoxService.cs` to Modal project
- Implement all interface methods
- Use TaskCompletionSource for async result handling

### Step 7: Create MessageBox.razor component
- Add `MessageBox.razor` to Modal project
- Render dialog with icon, message, and buttons
- Use ModalDialog CSS classes for consistent styling

### Step 8: Create MessageBox.razor.cs code-behind
- Implement parameters and OnResult callback
- Use ElementReference and JS interop for showModal/close
- Reuse ModalDialog.razor.js

### Step 9: Create MessageBox.razor.css styles
- Icon styles for each message type
- Body layout for icon + message
- Reuse modal-dialog classes where possible

### Step 10: Create MessageBoxProvider.razor component
- Subscribe to IMessageBoxService.OnShow
- Render MessageBox when triggered
- Pass result back to service

### Step 11: Create ServiceCollectionExtensions
- Add `AddMessageBox()` extension method
- Registers MessageBoxService as singleton

### Step 12: Create unit tests
- Test MessageBoxService methods
- Test MessageBox component rendering
- Test each MessageType renders correct icon
- Test each button configuration

### Step 13: Update sample page with MessageBox examples
- Add MessageBox examples to ModalDialogPage or new page
- Show each message type
- Show confirmation with result handling

### Step 14: Build and verify all tests pass

## Files to Create

### Core Project
- `src/D20Tek.BlazorComponent.Core/MessageType.cs`
- `src/D20Tek.BlazorComponent.Core/MessageBoxButtons.cs`
- `src/D20Tek.BlazorComponent.Core/MessageBoxResult.cs`

### Modal Project
- `src/D20Tek.BlazorComponent.Modal/MessageBoxOptions.cs`
- `src/D20Tek.BlazorComponent.Modal/IMessageBoxService.cs`
- `src/D20Tek.BlazorComponent.Modal/MessageBoxService.cs`
- `src/D20Tek.BlazorComponent.Modal/MessageBox.razor`
- `src/D20Tek.BlazorComponent.Modal/MessageBox.razor.cs`
- `src/D20Tek.BlazorComponent.Modal/MessageBox.razor.css`
- `src/D20Tek.BlazorComponent.Modal/MessageBoxProvider.razor`
- `src/D20Tek.BlazorComponent.Modal/ServiceCollectionExtensions.cs`

### Test Project
- `tests/D20Tek.BlazorComponents.UnitTests/Modal/MessageBoxTests.cs`
- `tests/D20Tek.BlazorComponents.UnitTests/Modal/MessageBoxServiceTests.cs`

## Files to Modify
- `samples/D20Tek.FullSample.Wasm/Program.cs` - Register service
- `samples/D20Tek.FullSample.Wasm/Shared/MainLayout.razor` - Add provider
- `samples/D20Tek.FullSample.Wasm/Pages/ModalDialogPage.razor` - Add examples

## Usage Examples

### Setup
```csharp
// Program.cs
builder.Services.AddMessageBox();
```

```razor
<!-- MainLayout.razor -->
@inherits LayoutComponentBase

<MessageBoxProvider />

<div class="page">
    @Body
</div>
```

### Show Simple Message
```razor
@inject IMessageBoxService MessageBox

<button @onclick="ShowInfo">Show Info</button>

@code {
    private async Task ShowInfo()
    {
        await MessageBox.ShowAsync("Your changes have been saved.", "Success", MessageType.Success);
    }
}
```

### Show Error Message
```razor
@inject IMessageBoxService MessageBox

@code {
    private async Task HandleError()
    {
        await MessageBox.ShowErrorAsync("Unable to connect to the server. Please try again.");
    }
}
```

### Confirmation Dialog
```razor
@inject IMessageBoxService MessageBox

<button @onclick="DeleteItem">Delete</button>

@code {
    private async Task DeleteItem()
    {
        var result = await MessageBox.ConfirmAsync(
            "Are you sure you want to delete this item? This action cannot be undone.",
            "Confirm Delete",
            MessageType.Warning,
            MessageBoxButtons.YesNo);

        if (result == MessageBoxResult.Yes)
        {
            // Perform delete
            await MessageBox.ShowSuccessAsync("Item deleted successfully.");
        }
    }
}
```

### Yes/No/Cancel Dialog
```razor
@inject IMessageBoxService MessageBox

@code {
    private async Task SaveChanges()
    {
        var result = await MessageBox.ConfirmAsync(
            "Do you want to save changes before closing?",
            "Unsaved Changes",
            MessageType.Question,
            MessageBoxButtons.YesNoCancel);

        switch (result)
        {
            case MessageBoxResult.Yes:
                await SaveDocument();
                CloseDocument();
                break;
            case MessageBoxResult.No:
                CloseDocument();
                break;
            case MessageBoxResult.Cancel:
                // Do nothing, stay on document
                break;
        }
    }
}
```

## Testing Strategy

### Unit Tests
1. **MessageBoxService tests**
   - ShowAsync raises OnShow event
   - ConfirmAsync returns correct result
   - SetResult completes TaskCompletionSource

2. **MessageBox component tests**
   - Renders correct icon for each MessageType
   - Renders correct buttons for each MessageBoxButtons value
   - OnResult fires with correct value when button clicked

3. **MessageBoxProvider tests**
   - Shows MessageBox when service raises OnShow
   - Hides MessageBox after result
   - Passes result back to service

### Integration Tests
- Full flow: inject service ? call ShowAsync ? click button ? verify result

## Dependencies
- `D20Tek.BlazorComponent.Core` - Base component, enums
- `Microsoft.AspNetCore.Components.Web` - Blazor support
- Reuses `ModalDialog.razor.js` for dialog show/close
- Reuses `ModalDialog.razor.css` for base dialog styling

## Future Enhancements (Out of Scope)
- Custom icon support
- Auto-close timeout
- Multiple message boxes in queue
- Toast-style notifications
- Customizable button text
