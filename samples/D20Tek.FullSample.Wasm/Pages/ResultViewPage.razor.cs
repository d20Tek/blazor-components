using D20Tek.BlazorComponents;
using D20Tek.Functional;
using D20Tek.FullSample.Wasm.Services;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ResultViewPage
{
    private Result<RegistrationModel>? _result;
    private bool _isLoading;
    private ResultViewState _currentState = ResultViewState.Empty;

    private void ShowSuccess()
    {
        _isLoading = false;
        _result = Result<RegistrationModel>.Success(
            new RegistrationModel { Name = "Ada Lovelace", Email = "ada@example.com" });
    }

    private void ShowFailure()
    {
        _isLoading = false;
        _result = Result<RegistrationModel>.Failure(
        [
            Error.Validation(nameof(RegistrationModel.Name), "Name is required."),
            Error.Validation(nameof(RegistrationModel.Email), "Email must be a valid email address.")
        ]);
    }

    private async Task ShowLoadingAsync()
    {
        _isLoading = true;
        _result = null;
        StateHasChanged();

        await Task.Delay(1000);

        _isLoading = false;
        ShowSuccess();
    }

    private void Reset()
    {
        _isLoading = false;
        _result = null;
    }

    private void HandleStateChanged(ResultViewState state) => _currentState = state;
}
