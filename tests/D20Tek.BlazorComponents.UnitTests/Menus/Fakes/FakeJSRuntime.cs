using Microsoft.JSInterop;

namespace D20Tek.BlazorComponents.UnitTests.Menus.Fakes;

internal sealed class FakeJSRuntime : IJSRuntime
{
    public List<string> Imports { get; } = [];

    public Exception? ThrowOnImport { get; set; }

    public FakeJSObjectReference Module { get; } = new();

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
    {
        Imports.Add(identifier);
        if (ThrowOnImport is not null)
        {
            throw ThrowOnImport;
        }

        return new ValueTask<TValue>((TValue)(object)Module);
    }

    [ExcludeFromCodeCoverage]
    public ValueTask<TValue> InvokeAsync<TValue>(
        string identifier,
        CancellationToken cancellationToken,
        object?[]? args) =>
        InvokeAsync<TValue>(identifier, args);
}
