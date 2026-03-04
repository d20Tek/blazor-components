using D20Tek.BlazorComponents;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ModalFormDialogPage
{
    private class UserFormModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
    }

    // Interactive dialog state
    private ModalFormDialog _interactiveDialog = default!;
    private string _title = "Sample Form Dialog";
    private string _summary = "Fill in the form details below.";
    private Size _size = Size.Medium;
    private VerticalPosition _position = VerticalPosition.Center;
    private string _submitText = "Submit";
    private string _cancelText = "Cancel";
    private bool _showCloseButton = true;
    private bool _showCancelButton = true;
    private string _lastAction = "None";
    private UserFormModel _interactiveModel = new();

    private async Task ShowInteractiveDialog() => await _interactiveDialog.ShowAsync();

    private async Task HandleInteractiveValidSubmit(EditContext context)
    {
        _lastAction = $"Submitted: {_interactiveModel.Name} at {DateTime.Now:HH:mm:ss}";
        await _interactiveDialog.CloseAsync();
        _interactiveModel = new();
    }

    private void HandleInteractiveCancel()
    {
        _lastAction = $"Cancelled at {DateTime.Now:HH:mm:ss}";
        _interactiveModel = new();
    }

    // Basic form dialog
    private ModalFormDialog _basicDialog = default!;
    private UserFormModel _basicModel = new();

    private async Task HandleBasicValidSubmit(EditContext context)
    {
        _lastAction = $"Basic form submitted: {_basicModel.Name}";
        await _basicDialog.CloseAsync();
        _basicModel = new();
    }

    private void HandleBasicCancel()
    {
        _lastAction = "Basic form cancelled";
        _basicModel = new();
    }

    // Validation form dialog
    private ModalFormDialog _validationDialog = default!;
    private UserFormModel _validationModel = new();

    private async Task HandleValidationValidSubmit(EditContext context)
    {
        _lastAction = $"Validated form submitted: {_validationModel.Name} ({_validationModel.Email})";
        await _validationDialog.CloseAsync();
        _validationModel = new();
    }

    private void HandleValidationInvalidSubmit(EditContext context) =>
        _lastAction = "Validation failed - check the form fields";

    private void HandleValidationCancel()
    {
        _lastAction = "Validation form cancelled";
        _validationModel = new();
    }

    // Large form dialog
    private ModalFormDialog _largeDialog = default!;
    private UserFormModel _largeModel = new();

    private async Task HandleLargeValidSubmit(EditContext context)
    {
        _lastAction = $"Large form submitted: {_largeModel.Name} ({_largeModel.Email})";
        await _largeDialog.CloseAsync();
        _largeModel = new();
    }

    private void HandleLargeCancel()
    {
        _lastAction = "Large form cancelled";
        _largeModel = new();
    }

    // Position variant dialogs
    private ModalFormDialog _topDialog = default!;
    private ModalFormDialog _centerDialog = default!;
    private ModalFormDialog _bottomDialog = default!;
    private UserFormModel _positionModel = new();

    private async Task HandleTopPositionSubmit(EditContext context)
    {
        _lastAction = "Top position form submitted";
        await _topDialog.CloseAsync();
        _positionModel = new();
    }

    private async Task HandleCenterPositionSubmit(EditContext context)
    {
        _lastAction = "Center position form submitted";
        await _centerDialog.CloseAsync();
        _positionModel = new();
    }

    private async Task HandleBottomPositionSubmit(EditContext context)
    {
        _lastAction = "Bottom position form submitted";
        await _bottomDialog.CloseAsync();
        _positionModel = new();
    }

    private void HandlePositionCancel()
    {
        _lastAction = "Position form cancelled";
        _positionModel = new();
    }
}
