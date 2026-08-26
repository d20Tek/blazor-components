using System.Text.RegularExpressions;
using D20Tek.Functional;

namespace D20Tek.FullSample.Wasm.Services;

public partial class RegistrationService
{
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    public async Task<Result<RegistrationModel>> RegisterAsync(RegistrationModel model)
    {
        // simulate an async API call.
        await Task.Delay(50);

        return ValidationErrors.Create()
            .AddIfError(
                () => string.IsNullOrWhiteSpace(model.Name),
                nameof(RegistrationModel.Name),
                "Name is required.")
            .AddIfError(
                () => string.IsNullOrWhiteSpace(model.Email),
                nameof(RegistrationModel.Email),
                "Email is required.")
            .AddIfError(
                () => !string.IsNullOrWhiteSpace(model.Email) && !EmailRegex().IsMatch(model.Email),
                nameof(RegistrationModel.Email),
                "Email must be a valid email address.")
            .Map(() => model);
    }
}
