using D20Tek.BlazorComponents;
using D20Tek.Functional;
using D20Tek.FullSample.Wasm.Services;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ResultValidatorPage
{
    private readonly RegistrationModel _model = new();
    private readonly BusyState _busy = new();
    private ResultValidator? _validator;
    private string _successMessage = string.Empty;
    private Result<RegistrationModel>? _lastResult;
    private Result<RegistrationModel>? _dismissibleResult;

    private async Task HandleSubmitAsync()
    {
        _successMessage = string.Empty;

        await _busy.RunAsync(async _ =>
        {
            var result = await RegistrationService.RegisterAsync(_model);
            _lastResult = result;
            _dismissibleResult = result;

            _validator?.HandleResult(
                result,
                onSuccess: value =>
                    _successMessage = $"Registered '{value.Name}' with email '{value.Email}'.");
        });
    }

    private void HandleAlertDismiss() => _dismissibleResult = null;
}
