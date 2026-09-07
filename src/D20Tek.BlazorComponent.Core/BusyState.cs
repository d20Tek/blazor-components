using System.Diagnostics.CodeAnalysis;

namespace D20Tek.BlazorComponents;

/// <summary>
/// Tracks a transient "busy"/"submitting" flag for a component and guarantees the
/// flag is reset when the returned scope is disposed, even if an exception is thrown.
/// </summary>
/// <example>
/// <code>
/// private readonly BusyState busy = new();
/// // markup: disabled="@busy.IsActive"
///
/// private async Task HandleSubmitAsync()
/// {
///     using var _ = busy.Begin();
///     await DoWorkAsync();
/// }
/// </code>
/// </example>
public sealed class BusyState
{
    private int _activeCount;

    /// <summary>
    /// Occurs when the <see cref="IsActive"/> state changes, allowing consumers
    /// (such as a Blazor component) to re-render in response.
    /// </summary>
    public event EventHandler? Changed;

    /// <summary>
    /// Gets a value indicating whether at least one busy scope is currently active.
    /// </summary>
    public bool IsActive => _activeCount > 0;

    /// <summary>
    /// Marks the state as busy and returns a scope that resets the busy flag when disposed.
    /// Nested calls are reference counted, so <see cref="IsActive"/> only becomes
    /// <see langword="false"/> once every scope has been disposed.
    /// </summary>
    /// <returns>A disposable scope that decrements the busy count when disposed.</returns>
    public IDisposable Begin()
    {
        var wasActive = IsActive;
        _activeCount++;
        if (!wasActive)
        {
            RaiseChanged();
        }

        return new Scope(this);
    }

    /// <summary>
    /// Attempts to begin a busy scope only when the state is not already active,
    /// which is useful for guarding against concurrent or double-submit operations.
    /// </summary>
    /// <param name="scope">
    /// When this method returns <see langword="true"/>, contains a disposable scope that
    /// resets the busy flag when disposed; otherwise <see langword="null"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the busy scope was started; <see langword="false"/> if
    /// the state was already active.
    /// </returns>
    public bool TryBegin([NotNullWhen(true)] out IDisposable? scope)
    {
        if (IsActive)
        {
            scope = null;
            return false;
        }

        scope = Begin();
        return true;
    }

    /// <summary>
    /// Runs the specified asynchronous operation within a busy scope, guaranteeing the
    /// busy flag is reset when the operation completes or throws.
    /// </summary>
    /// <param name="operation">The asynchronous operation to run while busy.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task that completes when the operation completes.</returns>
    public async Task RunAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        cancellationToken.ThrowIfCancellationRequested();

        using var _ = Begin();
        await operation(cancellationToken);
    }

    /// <summary>
    /// Runs the specified asynchronous operation within a busy scope and returns its result,
    /// guaranteeing the busy flag is reset when the operation completes or throws.
    /// </summary>
    /// <typeparam name="T">The type of value produced by the operation.</typeparam>
    /// <param name="operation">The asynchronous operation to run while busy.</param>
    /// <param name="cancellationToken">A token used to cancel the operation.</param>
    /// <returns>A task that completes with the operation's result.</returns>
    public async Task<T> RunAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);
        cancellationToken.ThrowIfCancellationRequested();

        using var _ = Begin();
        return await operation(cancellationToken);
    }

    internal void End()
    {
        if (_activeCount == 0) return;

        _activeCount--;
        if (!IsActive)
        {
            RaiseChanged();
        }
    }

    private void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);

    private sealed class Scope(BusyState owner) : IDisposable
    {
        private bool _disposed;

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            owner.End();
        }
    }
}
