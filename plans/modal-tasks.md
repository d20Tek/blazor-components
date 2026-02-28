# ModalDialog Component Implementation Plan

## Overview
Create a Blazor component called `ModalDialog` using the native HTML `<dialog>` element to minimize JavaScript requirements. The component will follow established patterns from the D20Tek.BlazorComponents library.

## Component Requirements

### Properties (Parameters)
| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `Title` | `string` | `""` | Modal dialog title displayed in header |
| `Summary` | `string` | `""` | Optional short summary/description below title |
| `ChildContent` | `RenderFragment` | - | Content rendered in the modal body |
| `IsVisible` | `bool` | `true` | Inherited from BaseComponent |
| `Size` | `Size` | `Size.Medium` | Modal size (inherited from BaseComponent) |
| `ShowCloseButton` | `bool` | `true` | Whether to show the X close button in header |
| `SubmitButtonText` | `string` | `"Submit"` | Text for submit/confirm button |
| `CancelButtonText` | `string` | `"Cancel"` | Text for cancel button |
| `ShowSubmitButton` | `bool` | `true` | Whether to show submit button |
| `ShowCancelButton` | `bool` | `true` | Whether to show cancel button |

### Events (EventCallbacks)
| Event | Type | Description |
|-------|------|-------------|
| `OnCancel` | `EventCallback` | Fired when Cancel button clicked or Escape pressed |
| `OnClose` | `EventCallback` | Fired when dialog is closed (X button or programmatic) |
| `OnSubmit` | `EventCallback` | Fired when Submit/Confirm button clicked |

### Public Methods
| Method | Description |
|--------|-------------|
| `ShowAsync()` | Opens the modal dialog using `showModal()` |
| `CloseAsync()` | Closes the modal dialog |

## Steps
  
1. Use the `D20Tek.BlazorComponents.Modal` project structure for ModalDialog component. It already has the required SDK and references.

2. Create GlobalUsings.cs file
   - Add standard global usings matching other component projects

3. Create ModalDialogSizeMetadata.cs
   - Define size-to-CSS mappings for dialog widths

4. Create ModalDialog.razor markup
   - Implement `<dialog>` element with header, body, footer sections
   - Use `@inherits BaseComponent` pattern
   - Wire up button click handlers

5. Create ModalDialog.razor.cs code-behind
   - Implement parameters and event callbacks
   - Override `CalculateCssClasses()` and `CalculateCssStyles()`
   - Implement JS interop for `showModal()` and `close()`

6. Create ModalDialog.razor.css scoped styles
   - Style dialog header with title and close button
   - Style dialog body for content
   - Style dialog footer with action buttons
   - Style native `::backdrop` pseudo-element

7. Create JavaScript interop module
   - Minimal JS for calling `showModal()` and `close()` on dialog element

8. Create _Imports.razor
   - Add required using statements

9. Add Modal reference to test project
    - Update test project references

10. Create unit tests for ModalDialog
    - Test default render
    - Test with Title and Summary
    - Test with custom button text
    - Test event callbacks
    - Test visibility toggling

11. Build and verify compilation
    - Run build to ensure no errors

## Files

### New Files to Create
- `src/D20Tek.BlazorComponents.Modal/D20Tek.BlazorComponents.Modal.csproj` (existing) - Project file
- `src/D20Tek.BlazorComponents.Modal/GlobalUsings.cs` (new) - Global using statements
- `src/D20Tek.BlazorComponents.Modal/_Imports.razor` (new) - Razor imports
- `src/D20Tek.BlazorComponents.Modal/ModalDialogSizeMetadata.cs` (new) - Size CSS mappings
- `src/D20Tek.BlazorComponents.Modal/ModalDialog.razor` (new) - Component markup
- `src/D20Tek.BlazorComponents.Modal/ModalDialog.razor.cs` (new) - Component code-behind
- `src/D20Tek.BlazorComponents.Modal/ModalDialog.razor.css` (new) - Scoped styles
- `src/D20Tek.BlazorComponents.Modal/wwwroot/modalDialog.js` (new) - JS interop module
- `tests/D20Tek.BlazorComponents.UnitTests/Modal/ModalDialogTests.cs` (new) - Unit tests

### Files to Modify
- `tests/D20Tek.BlazorComponents.UnitTests/D20Tek.BlazorComponents.UnitTests.csproj` (modify) - Add project reference

## Component Structure

```
ModalDialog.razor
├── <dialog> (native HTML5 dialog)
│   ├── <header class="modal-dialog__header">
│   │   ├── <h2 class="modal-dialog__title">@Title</h2>
│   │   ├── <p class="modal-dialog__summary">@Summary</p> (if not empty)
│   │   └── <button class="modal-dialog__close-btn">×</button> (if ShowCloseButton)
│   ├── <div class="modal-dialog__body">
│   │   └── @ChildContent
│   └── <footer class="modal-dialog__footer">
│       ├── <button class="modal-dialog__btn-cancel">@CancelButtonText</button>
│       └── <button class="modal-dialog__btn-submit">@SubmitButtonText</button>
```

## CSS Size Classes
| Size | CSS Class | Width |
|------|-----------|-------|
| ExtraSmall | `modal-dialog-xs` | 280px |
| Small | `modal-dialog-sm` | 400px |
| Medium | `modal-dialog-md` | 560px |
| Large | `modal-dialog-lg` | 720px |
| ExtraLarge | `modal-dialog-xl` | 900px |

## JavaScript Interop (Minimal)

```javascript
export function showModal(dialogId) {
    const dialog = document.getElementById(dialogId);
    if (dialog) dialog.showModal();
}

export function closeModal(dialogId) {
    const dialog = document.getElementById(dialogId);
    if (dialog) dialog.close();
}
```

## Native Dialog Benefits
- Built-in `::backdrop` pseudo-element for overlay styling
- Native keyboard support (Escape to close)
- Proper focus trapping
- Accessibility features (role="dialog", aria-modal)
- No z-index management needed

## Testing Strategy
1. **Render tests**: Verify correct HTML structure with various parameter combinations
2. **Event tests**: Verify callbacks fire correctly on button clicks
3. **Visibility tests**: Test `IsVisible` parameter behavior
4. **Size tests**: Verify correct CSS classes applied for each Size value
5. **Content tests**: Verify ChildContent renders correctly

## Dependencies
- `D20Tek.BlazorComponent.Core` - For BaseComponent, Size enum, CssBuilder, StyleBuilder
- `Microsoft.AspNetCore.Components.Web` - For Blazor Web support
- `Microsoft.JSInterop` - For JavaScript interop
