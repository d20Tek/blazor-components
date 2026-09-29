using D20Tek.BlazorComponents.UnitTests.Menus.Fakes;
using Microsoft.JSInterop;

namespace D20Tek.BlazorComponents.UnitTests.Menus.Shared;

[TestClass]
public class MenuInteropTests
{
    private static FlyoutMenu CreateOwner()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var comp = ctx.Render<FlyoutMenu>();
        return comp.Instance;
    }

    private static MenuInterop CreateInterop(FakeJSRuntime jsRuntime, out FlyoutMenu owner)
    {
        owner = CreateOwner();
        return new MenuInterop(jsRuntime, owner);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenModuleLoads_ImportsAndInvokesInitialize()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.InitializeAsync(default, default, new { });

        // Assert
        Assert.HasCount(1, jsRuntime.Imports);
        Assert.AreEqual("import", jsRuntime.Imports[0]);
        Assert.Contains("initialize", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenCalledTwice_ImportsModuleOnlyOnce()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.InitializeAsync(default, default, new { });
        await interop.InitializeAsync(default, default, new { });

        // Assert
        Assert.HasCount(1, jsRuntime.Imports);
        Assert.HasCount(2, jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenImportThrowsBenign_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime { ThrowOnImport = new JSDisconnectedException("disconnected") };
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.InitializeAsync(default, default, new { });

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenImportThrowsInvalidOperation_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime { ThrowOnImport = new InvalidOperationException("prerender") };
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.InitializeAsync(default, default, new { });

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InitializeAsync_WhenImportThrowsNonBenign_Rethrows()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime { ThrowOnImport = new ArgumentException("boom") };
        var interop = CreateInterop(jsRuntime, out _);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(
            [ExcludeFromCodeCoverage] async () => await interop.InitializeAsync(default, default, new { }));
    }

    [TestMethod]
    public async Task MoveFocusAsync_AfterInitialize_InvokesMoveFocus()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.Invocations.Clear();

        // Act
        await interop.MoveFocusAsync(default, "next");

        // Assert
        Assert.Contains("moveFocus", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task MoveFocusAsync_BeforeInitialize_DoesNothing()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.MoveFocusAsync(default, "next");

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task TypeAheadAsync_AfterInitialize_InvokesTypeAhead()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.Invocations.Clear();

        // Act
        await interop.TypeAheadAsync(default, "a");

        // Assert
        Assert.Contains("typeAhead", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task TeardownAsync_AfterInitialize_InvokesTeardown()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.Invocations.Clear();

        // Act
        await interop.TeardownAsync(default);

        // Assert
        Assert.Contains("teardown", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task FocusElementAsync_WhenElementReferenceThrowsBenign_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.FocusElementAsync(default);

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InvokeSafelyAsync_WhenModuleThrowsBenign_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.ThrowOnInvoke = new ObjectDisposedException("module");

        // Act
        await interop.MoveFocusAsync(default, "next");

        // Assert
        Assert.Contains("moveFocus", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task InvokeSafelyAsync_WhenModuleThrowsNonBenign_Rethrows()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.ThrowOnInvoke = new ArgumentException("boom");

        // Act & Assert
        await Assert.ThrowsExactlyAsync<ArgumentException>(
            [ExcludeFromCodeCoverage] async () => await interop.MoveFocusAsync(default, "next"));
    }

    [TestMethod]
    public async Task DisposeAsync_AfterInitialize_DisposesModuleAndInvokesDispose()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });

        // Act
        await interop.DisposeAsync(default);

        // Assert
        Assert.Contains("dispose", jsRuntime.Module.Invocations);
        Assert.IsTrue(jsRuntime.Module.Disposed);
    }

    [TestMethod]
    public async Task DisposeAsync_WhenModuleThrowsDisconnected_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.ThrowOnInvoke = new JSDisconnectedException("disconnected");

        // Act
        await interop.DisposeAsync(default);

        // Assert
        Assert.Contains("dispose", jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public async Task DisposeAsync_WhenModuleDisposeThrowsObjectDisposed_SwallowsException()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        jsRuntime.Module.ThrowOnDispose = new ObjectDisposedException("module");

        // Act
        await interop.DisposeAsync(default);

        // Assert
        Assert.IsTrue(jsRuntime.Module.Disposed);
    }

    [TestMethod]
    public async Task DisposeAsync_WithoutInitialize_DoesNotThrow()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);

        // Act
        await interop.DisposeAsync(default);

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
        Assert.IsFalse(jsRuntime.Module.Disposed);
    }

    [TestMethod]
    public async Task DisposeAsync_CalledTwice_SecondCallIsNoOp()
    {
        // Arrange
        var jsRuntime = new FakeJSRuntime();
        var interop = CreateInterop(jsRuntime, out _);
        await interop.InitializeAsync(default, default, new { });
        await interop.DisposeAsync(default);
        jsRuntime.Module.Invocations.Clear();

        // Act
        await interop.DisposeAsync(default);

        // Assert
        Assert.IsEmpty(jsRuntime.Module.Invocations);
    }

    [TestMethod]
    public void IsBenign_ForJSDisconnectedException_ReturnsTrue() =>
        Assert.IsTrue(MenuInterop.IsBenign(new JSDisconnectedException("x")));

    [TestMethod]
    public void IsBenign_ForObjectDisposedException_ReturnsTrue() =>
        Assert.IsTrue(MenuInterop.IsBenign(new ObjectDisposedException("x")));

    [TestMethod]
    public void IsBenign_ForInvalidOperationException_ReturnsTrue() =>
        Assert.IsTrue(MenuInterop.IsBenign(new InvalidOperationException("x")));

    [TestMethod]
    public void IsBenign_ForOtherException_ReturnsFalse() =>
        Assert.IsFalse(MenuInterop.IsBenign(new ArgumentException("x")));
}
