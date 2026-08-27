namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public class ResultExtensionsTests
{
    [TestMethod]
    public async Task HandleResultAsync_OnSuccess_InvokesOnSuccessWithValue()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("hello"));
        string? captured = null;

        // Act
        await task.HandleResultAsync(
            onSuccess: v => captured = v,
            onFailure: [ExcludeFromCodeCoverage](_) => Assert.Fail("onFailure should not be called"));

        // Assert
        Assert.AreEqual("hello", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_OnSuccess_DoesNotInvokeOnFailure()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(42));
        var failureCalled = false;

        // Act
        await task.HandleResultAsync(
            onSuccess: _ => { },
            onFailure: [ExcludeFromCodeCoverage] (_) => failureCalled = true);

        // Assert
        Assert.IsFalse(failureCalled);
    }

    [TestMethod]
    public async Task HandleResultAsync_OnFailure_InvokesOnFailureWithErrorMessage()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Failure(new InvalidOperationException("Something went wrong")));
        string? captured = null;

        // Act
        await task.HandleResultAsync(
            onSuccess: [ExcludeFromCodeCoverage] (_) => Assert.Fail("onSuccess should not be called"),
            onFailure: msg => captured = msg);

        // Assert
        Assert.IsNotNull(captured);
        Assert.Contains("Something went wrong", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_OnFailure_DoesNotInvokeOnSuccess()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Failure(new InvalidOperationException("fail")));
        var successCalled = false;

        // Act
        await task.HandleResultAsync(
            onSuccess: [ExcludeFromCodeCoverage] (_) => successCalled = true,
            onFailure: _ => { });

        // Assert
        Assert.IsFalse(successCalled);
    }

    [TestMethod]
    public async Task HandleResultAsync_OnFailure_UsesFirstErrorMessage()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Failure(new InvalidOperationException("First error")));
        string? captured = null;

        // Act
        await task.HandleResultAsync(
            onSuccess: [ExcludeFromCodeCoverage] (_) => { },
            onFailure: msg => captured = msg);

        // Assert
        Assert.IsNotNull(captured);
        Assert.Contains("First error", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_Async_OnSuccess_InvokesOnSuccessWithValue()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Success("hello"));
        string? captured = null;

        // Act
        await task.HandleResultAsync(
            onSuccess: v => { captured = v; return Task.CompletedTask; },
            onFailure: [ExcludeFromCodeCoverage] (_) => { Assert.Fail("onFailure should not be called"); return Task.CompletedTask; });

        // Assert
        Assert.AreEqual("hello", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_Async_OnSuccess_DoesNotInvokeOnFailure()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(42));
        var failureCalled = false;

        // Act
        await task.HandleResultAsync(
            onSuccess: _ => Task.CompletedTask,
            onFailure: [ExcludeFromCodeCoverage] (_) => { failureCalled = true; return Task.CompletedTask; });

        // Assert
        Assert.IsFalse(failureCalled);
    }

    [TestMethod]
    public async Task HandleResultAsync_Async_OnFailure_InvokesOnFailureWithErrorMessage()
    {
        // Arrange
        var task = Task.FromResult(Result<string>.Failure(new InvalidOperationException("Something went wrong")));
        string? captured = null;

        // Act
        await task.HandleResultAsync(
            onSuccess: [ExcludeFromCodeCoverage] (_) => { Assert.Fail("onSuccess should not be called"); return Task.CompletedTask; },
            onFailure: msg => { captured = msg; return Task.CompletedTask; });

        // Assert
        Assert.IsNotNull(captured);
        Assert.Contains("Something went wrong", captured);
    }

    [TestMethod]
    public async Task HandleResultAsync_Async_OnFailure_DoesNotInvokeOnSuccess()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Failure(new InvalidOperationException("fail")));
        var successCalled = false;

        // Act
        await task.HandleResultAsync(
            onSuccess: [ExcludeFromCodeCoverage] (_) => { successCalled = true; return Task.CompletedTask; },
            onFailure: _ => Task.CompletedTask);

        // Assert
        Assert.IsFalse(successCalled);
    }

    [TestMethod]
    public async Task HandleResultAsync_Async_OnSuccess_AwaitsCallback()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(10));
        var completed = false;

        // Act
        await task.HandleResultAsync(
            onSuccess: async _ => { await Task.Delay(1, CancellationToken.None); completed = true; },
            onFailure: [ExcludeFromCodeCoverage] (_) => Task.CompletedTask);

        // Assert
        Assert.IsTrue(completed);
    }
}
