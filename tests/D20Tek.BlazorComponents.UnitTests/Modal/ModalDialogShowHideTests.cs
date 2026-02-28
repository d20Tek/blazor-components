using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalDialogShowHideTests
{
    [TestMethod]
    public void IsOpen_DefaultIsFalse()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>();

        // assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task ShowAsync_SetsIsOpenToTrue()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();

        // act
        await comp.Instance.ShowAsync();

        // assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task CloseAsync_SetsIsOpenToFalse()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();
        await comp.Instance.ShowAsync();
        Assert.IsTrue(comp.Instance.IsOpen);

        // act
        await comp.Instance.CloseAsync();

        // assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task ShowAsync_ThenCloseAsync_TogglesIsOpen()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();
        Assert.IsFalse(comp.Instance.IsOpen);

        // act & assert - show
        await comp.Instance.ShowAsync();
        Assert.IsTrue(comp.Instance.IsOpen);

        // act & assert - close
        await comp.Instance.CloseAsync();
        Assert.IsFalse(comp.Instance.IsOpen);

        // act & assert - show again
        await comp.Instance.ShowAsync();
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task DisposeAsync_DisposesWithoutError()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();
        await comp.Instance.ShowAsync(); // This loads the JS module

        // act & assert - should not throw
        await comp.Instance.DisposeAsync();
    }

    [TestMethod]
    public async Task DisposeAsync_CanBeCalledMultipleTimes()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();
        await comp.Instance.ShowAsync();

        // act & assert - multiple dispose calls should not throw
        await comp.Instance.DisposeAsync();
        await comp.Instance.DisposeAsync();
    }

    [TestMethod]
    public async Task DisposeAsync_WorksWithoutShowAsync()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var comp = ctx.Render<ModalDialog>();
        // Note: ShowAsync not called, so no JS module loaded

        // act & assert - should not throw even when no module was loaded
        await comp.Instance.DisposeAsync();
    }
}
