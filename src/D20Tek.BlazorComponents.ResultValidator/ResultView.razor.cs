namespace D20Tek.BlazorComponents;

public partial class ResultView<T> : ComponentBase where T : notnull
{
    private Result<T>? _previousResult;
    private bool _previousLoading;

    [Parameter]
    public Result<T>? Result { get; set; }

    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public RenderFragment? Loading { get; set; }

    [Parameter]
    public RenderFragment<T>? Success { get; set; }

    [Parameter]
    public RenderFragment<IReadOnlyList<Error>>? Failure { get; set; }

    [Parameter]
    public RenderFragment? Empty { get; set; }

    [Parameter]
    public EventCallback<ResultViewState> OnStateChanged { get; set; }

    internal ResultViewState CurrentState =>
        this switch
        {
            { IsLoading: true } => ResultViewState.Loading,
            { Result: null } => ResultViewState.Empty,
            { Result.IsSuccess: true } => ResultViewState.Success,
            _ => ResultViewState.Failure
        };

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (!ReferenceEquals(_previousResult, Result) || _previousLoading != IsLoading)
        {
            _previousResult = Result;
            _previousLoading = IsLoading;
            await OnStateChanged.InvokeAsync(CurrentState);
        }
    }

    internal IReadOnlyList<Error> GetErrors() => Result is null || Result.IsSuccess ? [] : [.. Result.GetErrors()];
}
