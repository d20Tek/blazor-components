using D20Tek.BlazorComponents.Selectors;
using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.BlazorComponents;

public class ResultValidator : ComponentBase, IDisposable
{
    [CascadingParameter]
    private EditContext EditContext { get; set; } = null!;

    [Parameter]
    public IErrorFieldSelector? FieldSelector { get; set; }

    [Parameter]
    public UnknownFieldBehavior? UnknownFieldBehavior { get; set; }

    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = null!;

    private ValidationMessageStore _messageStore = null!;
    private ErrorMapper _errorMapper = null!;

    protected override void OnInitialized()
    {
        ArgumentNullException.ThrowIfNull(EditContext);

        EditContext.EnableDataAnnotationsValidation(ServiceProvider);
        _messageStore = new ValidationMessageStore(EditContext);
        _errorMapper = new ErrorMapper(_messageStore, EditContext);
        EditContext.OnFieldChanged += HandleFieldChanged;
    }

    public bool HandleResult<T>(
        Result<T> result,
        Action<T>? onSuccess = null,
        Func<Error, string>? fieldSelector = null) where T : notnull =>
        HandleResult(result, onSuccess, AsSelector(fieldSelector));

    public bool HandleResult<T>(
        Result<T> result,
        Action<T>? onSuccess,
        IErrorFieldSelector? fieldSelector) where T : notnull
    {
        if (!ProcessResult(result, fieldSelector)) return false;
        onSuccess?.Invoke(result.GetValue());
        return true;
    }

    public Task<bool> HandleResultAsync<T>(
        Result<T> result,
        Func<T, Task>? onSuccess = null,
        Func<Error, string>? fieldSelector = null) where T : notnull =>
        HandleResultAsync(result, onSuccess, AsSelector(fieldSelector));

    public async Task<bool> HandleResultAsync<T>(
        Result<T> result,
        Func<T, Task>? onSuccess,
        IErrorFieldSelector? fieldSelector) where T : notnull
    {
        if (!ProcessResult(result, fieldSelector)) return false;
        if (onSuccess is not null) await onSuccess(result.GetValue());
        return true;
    }

    private bool ProcessResult<T>(Result<T> result, IErrorFieldSelector? perCallSelector) where T : notnull
    {
        _messageStore.Clear();

        if (result.IsSuccess) return true;

        MapErrors(result.GetErrors(), perCallSelector);
        EditContext.NotifyValidationStateChanged();
        return false;
    }

    public void ClearErrors()
    {
        _messageStore.Clear();
        EditContext.NotifyValidationStateChanged();
    }

    private void MapErrors(IEnumerable<Error> errors, IErrorFieldSelector? perCallSelector) =>
        _errorMapper.Map(errors, perCallSelector ?? ResolveSelector(), ResolveUnknownBehavior());

    private static DelegateFieldSelector? AsSelector(Func<Error, string>? func) =>
        func is null ? null : new DelegateFieldSelector(func);

    private IErrorFieldSelector ResolveSelector()
    {
        if (FieldSelector is not null) return FieldSelector;

        var injected = ServiceProvider!.GetService<IErrorFieldSelector>();
        return injected ?? CodeAsFieldSelector.Instance;
    }

    private UnknownFieldBehavior ResolveUnknownBehavior()
    {
        if (UnknownFieldBehavior is not null) return UnknownFieldBehavior.Value;

        var injectedOptions = ServiceProvider!.GetService<ResultValidatorOptions>();
        return injectedOptions?.UnknownFieldBehavior ?? BlazorComponents.UnknownFieldBehavior.PassThrough;
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
