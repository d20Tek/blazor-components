namespace D20Tek.BlazorComponents;

public static class ResultToastExtensions
{
    public static ToastInstance ShowResult<T>(
        this IToastService toastService,
        Result<T> result,
        Action<ResultToastOptions<T>>? configure = null)
        where T : notnull
    {
        ArgumentNullException.ThrowIfNull(toastService);
        ArgumentNullException.ThrowIfNull(result);

        var options = new ResultToastOptions<T>();
        configure?.Invoke(options);

        return result.IsSuccess
            ? ShowSuccess(toastService, result.GetValue(), options)
            : ShowFailure(toastService, [.. result.GetErrors()], options);
    }

    private static ToastInstance ShowSuccess<T>(
        IToastService toastService,
        T value,
        ResultToastOptions<T> options)
        where T : notnull
    {
        var message = options.SuccessMessage
            ?? options.SuccessFormatter?.Invoke(value)
            ?? "Operation completed successfully.";

        return toastService.Show(
            BuildTextFragment(message),
            toast => ApplyOptions(toast, options, NotificationVariant.Success, options.SuccessTimeout));
    }

    private static ToastInstance ShowFailure<T>(
        IToastService toastService,
        IReadOnlyList<Error> errors,
        ResultToastOptions<T> options)
        where T : notnull
    {
        var messages = GetErrorMessages(errors, options);

        return toastService.Show(
            BuildFailureFragment(options.FailureMessage, messages),
            toast => ApplyOptions(toast, options, NotificationVariant.Error, options.FailureTimeout));
    }

    private static IReadOnlyList<string> GetErrorMessages<T>(
        IReadOnlyList<Error> errors,
        ResultToastOptions<T> options)
        where T : notnull
    {
        IEnumerable<Error> filtered = errors;
        if (options.GroupErrorsByCode)
        {
            filtered = filtered.GroupBy(e => e.Code).Select(g => g.First());
        }

        if (options.MaxErrorsShown is int max && max >= 0)
        {
            filtered = filtered.Take(max);
        }

        return [.. filtered.Select(options.ErrorFormatter)];
    }

    private static void ApplyOptions<T>(
        ToastOptions toast,
        ResultToastOptions<T> options,
        NotificationVariant variant,
        TimeSpan timeout)
        where T : notnull
    {
        toast.Variant = variant;
        toast.Position = options.Position;
        toast.Timeout = timeout;
        toast.ShowIcon = options.ShowIcon;
        toast.Dismissible = options.Dismissible;
        toast.Animate = options.Animate;
    }

    private static RenderFragment BuildTextFragment(string message) => builder =>
        builder.AddContent(0, message);

    private static RenderFragment BuildFailureFragment(
        string header,
        IReadOnlyList<string> messages) => builder =>
    {
        var seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "result-toast");

        if (!string.IsNullOrEmpty(header))
        {
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "result-toast__header");
            builder.AddContent(seq++, header);
            builder.CloseElement();
        }

        if (messages.Count > 0)
        {
            builder.OpenElement(seq++, "ul");
            builder.AddAttribute(seq++, "class", "result-toast__errors");
            foreach (var message in messages)
            {
                builder.OpenElement(seq++, "li");
                builder.AddContent(seq++, message);
                builder.CloseElement();
            }

            builder.CloseElement();
        }

        builder.CloseElement();
    };
}
