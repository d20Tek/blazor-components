using D20Tek.BlazorComponents;
using D20Tek.Functional;
using D20Tek.FullSample.Wasm.Services;
using Microsoft.AspNetCore.Components;

namespace D20Tek.FullSample.Wasm.Pages;

public partial class ToastPage
{
    private NotificationVariant _variant = NotificationVariant.Info;
    private ToastPosition _position = ToastPosition.BottomCenter;
    private bool _sticky;

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    private void ShowToast() =>
        ToastService.Show($"This is a {_variant} toast.", _variant, options =>
        {
            options.Position = _position;
            options.Timeout = _sticky ? TimeSpan.Zero : TimeSpan.FromSeconds(5);
        });

    private void ShowMultiple()
    {
        for (var i = 1; i <= 3; i++)
        {
            var index = i;
            ToastService.Show($"Stacked toast #{index}.", _variant, options =>
            {
                options.Position = _position;
                options.Timeout = _sticky ? TimeSpan.Zero : TimeSpan.FromSeconds(5);
            });
        }
    }

    private void ShowResultSuccess()
    {
        var result = Result<RegistrationModel>.Success(
            new RegistrationModel { Name = "Ada Lovelace", Email = "ada@example.com" });

        ToastService.ShowResult(result, options =>
        {
            options.Position = _position;
            options.SuccessFormatter = m => $"Registered {m.Name} successfully.";
        });
    }

    private void ShowResultFailure()
    {
        var result = Result<RegistrationModel>.Failure(
        [
            Error.Validation(nameof(RegistrationModel.Name), "Name is required."),
            Error.Validation(nameof(RegistrationModel.Email), "Email must be a valid email address.")
        ]);

        ToastService.ShowResult(result, options =>
        {
            options.Position = _position;
            options.FailureMessage = "Registration failed:";
        });
    }
}
