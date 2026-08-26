namespace D20Tek.BlazorComponents;

public class ResultValidator : ComponentBase, IDisposable
{
    [CascadingParameter]
    private EditContext EditContext { get; set; } = null!;

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;

    private ValidationMessageStore _messageStore = null!;

    protected override void OnInitialized()
    {
        ArgumentNullException.ThrowIfNull(EditContext);

        EditContext.EnableDataAnnotationsValidation(ServiceProvider);
        _messageStore = new ValidationMessageStore(EditContext);
        EditContext.OnFieldChanged += HandleFieldChanged;
    }

    public bool HandleResult<T>(
        Result<T> result,
        Action<T>? onSuccess = null,
        Func<Error, string>? fieldSelector = null) where T : notnull
    {
        _messageStore.Clear();

        if (result.IsSuccess)
        {
            onSuccess?.Invoke(result.GetValue());
            return true;
        }

        foreach (var error in result.GetErrors())
        {
            var fieldName = fieldSelector is not null ? fieldSelector(error) : DefaultFieldSelector(error);
            _messageStore.Add(EditContext.Field(fieldName), error.Message);
        }

        EditContext.NotifyValidationStateChanged();
        return false;
    }

    public async Task<bool> HandleResultAsync<T>(
        Result<T> result,
        Func<T, Task>? onSuccess = null,
        Func<Error, string>? fieldSelector = null) where T : notnull
    {
        _messageStore.Clear();

        if (result.IsSuccess)
        {
            if (onSuccess is not null)
                await onSuccess(result.GetValue());
            return true;
        }

        foreach (var error in result.GetErrors())
        {
            var fieldName = fieldSelector is not null ? fieldSelector(error) : DefaultFieldSelector(error);
            _messageStore.Add(EditContext.Field(fieldName), error.Message);
        }

        EditContext.NotifyValidationStateChanged();
        return false;
    }

    private static string DefaultFieldSelector(Error error) => 
        error.Type is ErrorType.Validation ? error.Code : string.Empty;

    public void ClearErrors()
    {
        _messageStore.Clear();
        EditContext.NotifyValidationStateChanged();
    }

    private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        _messageStore.Clear(e.FieldIdentifier);
        EditContext.NotifyValidationStateChanged();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        EditContext.OnFieldChanged -= HandleFieldChanged;
    }
}
