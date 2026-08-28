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
        where T : notnull =>
        toastService.Show(
            ResultToastFragmentBuilder.BuildTextFragment(GetSuccessMessage(value, options)),
            toast => ApplyOptions(toast, options, NotificationVariant.Success, options.SuccessTimeout));

    private static string GetSuccessMessage<T>(T value, ResultToastOptions<T> options) where T : notnull =>
         options.SuccessMessage ?? options.SuccessFormatter?.Invoke(value) ?? "Operation completed successfully.";

    private static ToastInstance ShowFailure<T>(
        IToastService toastService,
        IReadOnlyList<Error> errors,
        ResultToastOptions<T> options)
        where T : notnull =>
        toastService.Show(
            ResultToastFragmentBuilder.BuildFailureFragment(options.FailureMessage, GetErrorMessages(errors, options)),
            toast => ApplyOptions(toast, options, NotificationVariant.Error, options.FailureTimeout));

    private static IReadOnlyList<string> GetErrorMessages<T>(IReadOnlyList<Error> errors, ResultToastOptions<T> options)
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
}
