namespace D20Tek.BlazorComponents;

public partial class ResultAlert<T> : BaseComponent where T : notnull
{
    private Result<T>? _previousResult;
    private bool _dismissed;

    [Parameter]
    public Result<T>? Result { get; set; }

    [Parameter]
    public RenderFragment<T>? SuccessContent { get; set; }

    [Parameter]
    public RenderFragment<IReadOnlyList<Error>>? ErrorContent { get; set; }

    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    [Parameter]
    public RenderFragment? SuccessIcon { get; set; }

    [Parameter]
    public RenderFragment? ErrorIcon { get; set; }

    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    [Parameter]
    public Func<Error, string> ErrorFormatter { get; set; } = e => e.Message;

    [Parameter]
    public bool GroupErrorsByCode { get; set; }

    [Parameter]
    public int? MaxErrorsShown { get; set; }

    [Parameter]
    public bool ShowOnSuccess { get; set; } = true;

    [Parameter]
    public bool ShowOnFailure { get; set; } = true;

    [Parameter]
    public bool ShowCloseButton { get; set; }

    [Parameter]
    public EventCallback OnDismiss { get; set; }

    [Parameter]
    public AlertVariant? Variant { get; set; }

    [Parameter]
    public AlertVariant? SuccessVariant { get; set; }

    [Parameter]
    public AlertVariant? FailureVariant { get; set; }

    [Parameter]
    public bool ShowIcon { get; set; } = true;

    [Parameter]
    public bool Bordered { get; set; }

    [Parameter]
    public bool Elevated { get; set; }

    [Parameter]
    public bool Animate { get; set; } = true;

    [Parameter]
    public string CloseButtonAriaLabel { get; set; } = "Dismiss";

    [Parameter]
    public EventCallback<Result<T>> OnResultRendered { get; set; }

    [Parameter]
    public EventCallback<IReadOnlyList<Error>> OnErrorsShown { get; set; }

    [Parameter]
    public EventCallback OnSuccessShown { get; set; }

    private bool IsSuccess => Result?.IsSuccess ?? false;

    private string AriaLive => IsSuccess ? "polite" : "assertive";

    private AlertVariant CurrentVariant =>
        Variant ??
        (IsSuccess
            ? SuccessVariant ?? AlertVariant.Success
            : FailureVariant ?? AlertVariant.Error);

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (!ReferenceEquals(_previousResult, Result))
        {
            _previousResult = Result;
            _dismissed = false;

            if (Result is not null)
            {
                await OnResultRendered.InvokeAsync(Result);

                if (Result.IsSuccess)
                {
                    await OnSuccessShown.InvokeAsync();
                }
                else
                {
                    await OnErrorsShown.InvokeAsync(GetErrors());
                }
            }
        }
    }

    private bool ShouldShowAlert()
    {
        if (!IsVisible || _dismissed || Result is null) return false;
        return IsSuccess ? ShowOnSuccess : ShowOnFailure;
    }

    internal IReadOnlyList<Error> GetErrors()
    {
        if (Result is null || Result.IsSuccess) return [];

        IEnumerable<Error> errors = Result.GetErrors();
        if (GroupErrorsByCode)
        {
            errors = errors.GroupBy(e => e.Code).Select(g => g.First());
        }

        return [.. errors];
    }

    private IReadOnlyList<Error> GetVisibleErrors()
    {
        var errors = GetErrors();
        if (MaxErrorsShown is int max && max >= 0 && errors.Count > max)
        {
            return [.. errors.Take(max)];
        }

        return errors;
    }

    private int OverflowCount()
    {
        if (MaxErrorsShown is not int max || max < 0) return 0;
        var total = GetErrors().Count;
        return total > max ? total - max : 0;
    }

    private async Task HandleDismiss()
    {
        _dismissed = true;
        await OnDismiss.InvokeAsync();
    }

    private RenderFragment DefaultIcon() => builder =>
        builder.AddMarkupContent(0, ResultAlertVariantMetadata.GetDefaultIcon(CurrentVariant));

    protected override string? CalculateCssClasses() =>
        new CssBuilder("result-alert")
            .AddClass(ResultAlertVariantMetadata.GetVariantCss(CurrentVariant))
            .AddClass(ResultAlertSizeMetadata.GetSizeCss(Size), Size != Size.None)
            .AddClass("result-alert-bordered", Bordered)
            .AddClass("result-alert-elevated", Elevated)
            .AddClass("result-alert-animated", Animate)
            .AddClass("result-alert-dismissible", ShowCloseButton)
            .AddClassFromAttributes(RemainingAttributes)
            .Build();

    protected override string? CalculateCssStyles() =>
        new StyleBuilder()
            .AddStyleFromAttributes(RemainingAttributes)
            .Build();
}
