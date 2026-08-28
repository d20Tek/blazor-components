namespace D20Tek.BlazorComponents;

public static class ResultExtensions
{
    public static async Task HandleResultAsync<T>(this Task<Result<T>> result, Action<T> onSuccess, Action<string> onFailure)
        where T : notnull
    {
        var r = await result;
        if (r.IsSuccess)
            onSuccess(r.GetValue());
        else
            onFailure(r.GetErrors().First().ToString());
    }

    public static async Task HandleResultAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess, Func<string, Task> onFailure)
        where T : notnull
    {
        var r = await result;
        if (r.IsSuccess)
            await onSuccess(r.GetValue());
        else
            await onFailure(r.GetErrors().First().ToString());
    }
}
