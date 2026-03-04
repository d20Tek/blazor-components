using D20Tek.BlazorComponents;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ModalDialogPage
{
    // Interactive dialog state
    private ModalDialog _interactiveDialog = default!;
    private string _title = "Sample Dialog";
    private string _summary = "This is a sample modal dialog.";
    private Size _size = Size.Medium;
    private VerticalPosition _position = VerticalPosition.Center;
    private string _submitText = "Submit";
    private string _cancelText = "Cancel";
    private bool _showCloseButton = true;
    private bool _showSubmitButton = true;
    private bool _showCancelButton = true;
    private string _lastAction = "None";

    // Example dialog references
    private ModalDialog _basicDialog = default!;
    private ModalDialog _confirmDialog = default!;
    private ModalDialog _infoDialog = default!;
    private ModalDialog _largeDialog = default!;
    private ModalDialog _topDialog = default!;
    private ModalDialog _centerDialog = default!;
    private ModalDialog _bottomDialog = default!;

    private async Task ShowInteractiveDialog() => await _interactiveDialog.ShowAsync();

    private void HandleClose() => _lastAction = $"Closed at {DateTime.Now:HH:mm:ss}";
    private void HandleSubmit() => _lastAction = $"Submitted at {DateTime.Now:HH:mm:ss}";

    private void HandleBasicClose() => _lastAction = "Basic dialog closed";

    private void HandleBasicSubmit() => _lastAction = "Basic dialog submitted";

    private void HandleConfirmClose() => _lastAction = "Delete cancelled";

    private void HandleConfirmSubmit() => _lastAction = "Item deleted!";

    private void HandleInfoClose() => _lastAction = "Info dialog closed";

    private void HandleLargeClose() => _lastAction = "Terms declined";

    private void HandleLargeSubmit() => _lastAction = "Terms accepted!";

    private void HandlePositionClose() => _lastAction = "Position dialog closed";

    private void HandlePositionSubmit() => _lastAction = "Position dialog submitted";

    // Form dialog state
    private class UserFormModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
    }

    private ModalFormDialog _formDialog = default!;
    private UserFormModel _formModel = new();

    private async Task HandleFormSubmit(EditContext context)
    {
        _lastAction = $"Form submitted: {_formModel.Name} ({_formModel.Email})";
        await _formDialog.CloseAsync();
        _formModel = new();
    }

    private void HandleFormCancel()
    {
        _lastAction = "Form cancelled";
        _formModel = new();
    }
}
