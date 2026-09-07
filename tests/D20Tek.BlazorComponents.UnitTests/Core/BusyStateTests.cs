namespace D20Tek.BlazorComponents.UnitTests.Core;

[TestClass]
public sealed class BusyStateTests
{
    [TestMethod]
    public void IsActive_WhenNewlyCreated_IsFalse()
    {
        // arrange
        var busy = new BusyState();

        // act
        var result = busy.IsActive;

        // assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Begin_WhenCalled_SetsIsActiveTrue()
    {
        // arrange
        var busy = new BusyState();

        // act
        using var scope = busy.Begin();

        // assert
        Assert.IsTrue(busy.IsActive);
    }

    [TestMethod]
    public void Begin_WhenScopeDisposed_ResetsIsActiveFalse()
    {
        // arrange
        var busy = new BusyState();

        // act
        using (busy.Begin())
        {
        }

        // assert
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public void Begin_WhenNested_StaysActiveUntilLastScopeDisposed()
    {
        // arrange
        var busy = new BusyState();

        // act - assert
        var outer = busy.Begin();
        var inner = busy.Begin();

        inner.Dispose();
        Assert.IsTrue(busy.IsActive, "Should remain active while the outer scope is open.");

        outer.Dispose();
        Assert.IsFalse(busy.IsActive, "Should be inactive after the last scope is disposed.");
    }

    [TestMethod]
    public void Dispose_WhenCalledTwice_DoesNotDecrementBelowZero()
    {
        // arrange
        var busy = new BusyState();
        var _ = busy.Begin();
        var inner = busy.Begin();

        // act
        inner.Dispose();
        inner.Dispose();

        // assert
        Assert.IsTrue(busy.IsActive);
    }

    [TestMethod]
    public void Begin_WhenActivated_RaisesChangedOnce()
    {
        // arrange
        var busy = new BusyState();
        var count = 0;
        busy.Changed += (_, _) => count++;

        // act
        _ = busy.Begin();

        // assert
        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void Begin_WhenNested_RaisesChangedOnlyOnFirstActivation()
    {
        // arrange
        var busy = new BusyState();
        var count = 0;
        busy.Changed += (_, _) => count++;

        // act
        _ = busy.Begin();
        _ = busy.Begin();

        // assert
        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public void Dispose_WhenLastScopeDisposed_RaisesChangedOnce()
    {
        // arrange
        var busy = new BusyState();
        var count = 0;
        var scope = busy.Begin();
        busy.Changed += (_, _) => count++;

        // act
        scope.Dispose();

        // assert
        Assert.AreEqual(1, count);
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public void Changed_WhenRaised_PassesBusyStateAsSender()
    {
        // arrange
        var busy = new BusyState();
        object? sender = null;
        busy.Changed += (s, _) => sender = s;

        // act
        _ = busy.Begin();

        // assert
        Assert.AreSame(busy, sender);
    }

    [TestMethod]
    public void TryBegin_WhenNotActive_ReturnsTrueAndScope()
    {
        // arrange
        var busy = new BusyState();

        // act
        var result = busy.TryBegin(out var scope);

        // assert
        Assert.IsTrue(result);
        Assert.IsNotNull(scope);
        Assert.IsTrue(busy.IsActive);
    }

    [TestMethod]
    public void TryBegin_WhenAlreadyActive_ReturnsFalseAndNullScope()
    {
        // arrange
        var busy = new BusyState();
        using var first = busy.Begin();

        // act
        var result = busy.TryBegin(out var scope);

        // assert
        Assert.IsFalse(result);
        Assert.IsNull(scope);
    }

    [TestMethod]
    public void TryBegin_WhenScopeDisposed_AllowsSubsequentTryBegin()
    {
        // arrange
        var busy = new BusyState();
        busy.TryBegin(out var scope);
        scope!.Dispose();

        // act
        var result = busy.TryBegin(out var second);

        // assert
        Assert.IsTrue(result);
        Assert.IsNotNull(second);
    }

    [TestMethod]
    public async Task RunAsync_WhileRunning_IsActiveThenResets()
    {
        // arrange
        var busy = new BusyState();
        var wasActiveDuringRun = false;

        // act
        await busy.RunAsync(_ =>
        {
            wasActiveDuringRun = busy.IsActive;
            return Task.CompletedTask;
        }, CancellationToken.None);

        // assert
        Assert.IsTrue(wasActiveDuringRun);
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public async Task RunAsync_WhenOperationThrows_ResetsIsActive()
    {
        // arrange
        var busy = new BusyState();

        // act
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(
            [ExcludeFromCodeCoverage] () => busy.RunAsync(_ => throw new InvalidOperationException(), CancellationToken.None));

        // assert
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public async Task RunAsync_WhenOperationNull_Throws()
    {
        // arrange
        var busy = new BusyState();

        // act - assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => busy.RunAsync(null!, CancellationToken.None));
    }

    [TestMethod]
    public async Task RunAsync_WhenTokenAlreadyCancelled_ThrowsAndDoesNotActivate()
    {
        // arrange
        var busy = new BusyState();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // act - assert
        await Assert.ThrowsExactlyAsync<OperationCanceledException>(
            [ExcludeFromCodeCoverage] () => busy.RunAsync([ExcludeFromCodeCoverage](_) => Task.CompletedTask, cts.Token));
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public async Task RunAsync_PassesTokenToOperation()
    {
        // arrange
        var busy = new BusyState();
        using var cts = new CancellationTokenSource();
        CancellationToken received = default;

        // act
        await busy.RunAsync(token =>
        {
            received = token;
            return Task.CompletedTask;
        }, cts.Token);

        // assert
        Assert.AreEqual(cts.Token, received);
    }

    [TestMethod]
    public async Task RunAsyncOfT_WhileRunning_ReturnsResultAndResets()
    {
        // arrange
        var busy = new BusyState();
        var wasActiveDuringRun = false;

        // act
        var result = await busy.RunAsync(_ =>
        {
            wasActiveDuringRun = busy.IsActive;
            return Task.FromResult(42);
        }, CancellationToken.None);

        // assert
        Assert.AreEqual(42, result);
        Assert.IsTrue(wasActiveDuringRun);
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public async Task RunAsyncOfT_WhenOperationThrows_ResetsIsActive()
    {
        // arrange
        var busy = new BusyState();

        // act
        await Assert.ThrowsExactlyAsync<InvalidOperationException>([ExcludeFromCodeCoverage] () => 
            busy.RunAsync<int>(_ => throw new InvalidOperationException(), CancellationToken.None));

        // assert
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public async Task RunAsyncOfT_WhenOperationNull_Throws()
    {
        // arrange
        var busy = new BusyState();

        // act - assert
        await Assert.ThrowsExactlyAsync<ArgumentNullException>(
            [ExcludeFromCodeCoverage] () => busy.RunAsync<int>(null!, CancellationToken.None));
    }

    [TestMethod]
    public async Task RunAsyncOfT_WhenTokenAlreadyCancelled_ThrowsAndDoesNotActivate()
    {
        // arrange
        var busy = new BusyState();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // act - assert
        await Assert.ThrowsExactlyAsync<OperationCanceledException>(
            [ExcludeFromCodeCoverage] () => busy.RunAsync([ExcludeFromCodeCoverage](_) => Task.FromResult(1), cts.Token));
        Assert.IsFalse(busy.IsActive);
    }

    [TestMethod]
    public void End_WhenCountIsZero_IsNoOp()
    {
        // arrange
        var busy = new BusyState();
        var changedRaised = false;
        busy.Changed += [ExcludeFromCodeCoverage](_, _) => changedRaised = true;

        // act
        busy.End();

        // assert
        Assert.IsFalse(busy.IsActive);
        Assert.IsFalse(changedRaised);
    }
}
