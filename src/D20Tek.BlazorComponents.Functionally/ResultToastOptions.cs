namespace D20Tek.BlazorComponents;

public sealed class ResultToastOptions<T> where T : notnull
{
    public string? SuccessMessage { get; set; }

    public string FailureMessage { get; set; } = "An error occurred.";

    public Func<T, string>? SuccessFormatter { get; set; }

    public Func<Error, string> ErrorFormatter { get; set; } = e => e.Message;

    public bool GroupErrorsByCode { get; set; }

    public int? MaxErrorsShown { get; set; }

    public ToastPosition Position { get; set; } = ToastPosition.BottomCenter;

    public TimeSpan SuccessTimeout { get; set; } = TimeSpan.FromSeconds(5);

    public TimeSpan FailureTimeout { get; set; } = TimeSpan.Zero;

    public bool ShowIcon { get; set; } = true;

    public bool Dismissible { get; set; } = true;

    public bool Animate { get; set; } = true;
}
