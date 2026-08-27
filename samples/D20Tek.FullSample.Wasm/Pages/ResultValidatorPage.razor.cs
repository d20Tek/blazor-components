using D20Tek.BlazorComponents;
using D20Tek.Functional;
using D20Tek.FullSample.Wasm.Services;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ResultValidatorPage
{
    private readonly RegistrationModel _model = new();
    private ResultValidator? _validator;
    private string _successMessage = string.Empty;
    private bool _submitting;
    private Result<RegistrationModel>? _lastResult;
    private Result<RegistrationModel>? _dismissibleResult;

    private async Task HandleSubmitAsync()
    {
        _successMessage = string.Empty;
        _submitting = true;

        try
        {
            var result = await RegistrationService.RegisterAsync(_model);
            _lastResult = result;
            _dismissibleResult = result;

            _validator?.HandleResult(
                result,
                onSuccess: value =>
                    _successMessage = $"Registered '{value.Name}' with email '{value.Email}'.");
        }
        finally
        {
            _submitting = false;
        }
    }

    private void HandleAlertDismiss() => _dismissibleResult = null;
}
