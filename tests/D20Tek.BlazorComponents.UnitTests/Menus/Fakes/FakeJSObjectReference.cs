using Microsoft.JSInterop;

namespace D20Tek.BlazorComponents.UnitTests.Menus.Fakes;

internal sealed class FakeJSObjectReference : IJSObjectReference
{
    public List<string> Invocations { get; } = [];

    public Exception? ThrowOnInvoke { get; set; }

    public Exception? ThrowOnDispose { get; set; }

    public bool Disposed { get; private set; }

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    {
        Invocations.Add(identifier);
        if (ThrowOnInvoke is not null)
        {
            throw ThrowOnInvoke;
        }

        return new ValueTask<TValue>(default(TValue)!);
    }

    [ExcludeFromCodeCoverage]
    public ValueTask<TValue> InvokeAsync<TValue>(
        string identifier,
        CancellationToken cancellationToken,
        object?[]? args) =>
        InvokeAsync<TValue>(identifier, args);

    public ValueTask DisposeAsync()
    {
        Disposed = true;
        if (ThrowOnDispose is not null)
        {
            throw ThrowOnDispose;
        }

        return ValueTask.CompletedTask;
    }
}
