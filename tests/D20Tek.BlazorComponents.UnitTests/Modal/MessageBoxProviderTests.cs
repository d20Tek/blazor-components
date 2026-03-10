using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxProviderTests
{
    [TestMethod]
    public void InitialRender_DoesNotShowMessageBox()
    {
        // arrange
        var ctx = new BunitContext();
        var services = ctx.Services;
        services.AddMessageBox();

        // act
        var comp = ctx.Render<MessageBoxProvider>();

        // assert
        var messageBoxes = comp.FindComponents<MessageBox>();
        Assert.IsEmpty(messageBoxes);
    }

    [TestMethod]
    public void ServiceOnShowEvent_RendersMessageBox()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        _ = service!.ShowAsync("Test message", "Test Title");

        // assert
        var messageBoxes = comp.FindComponents<MessageBox>();
        Assert.HasCount(1, messageBoxes);
        Assert.AreEqual("Test message", messageBoxes[0].Instance.Message);
        Assert.AreEqual("Test Title", messageBoxes[0].Instance.Title);
    }

    [TestMethod]
    public async Task MessageBoxResult_HidesDialog()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>() as IMessageBoxService;
        var task = service!.ShowAsync("Test message");

        // act
        var messageBox = comp.FindComponent<MessageBox>();
        await messageBox.InvokeAsync(() => messageBox.Instance.OnResult.InvokeAsync(MessageBoxResult.Ok));

        // assert
        var messageBoxes = comp.FindComponents<MessageBox>();
        Assert.IsEmpty(messageBoxes);
    }

    [TestMethod]
    public async Task MessageBoxResult_PassesResultToService()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>() as IMessageBoxService;
        var task = service!.ConfirmAsync("Confirm?");

        // act
        var messageBox = comp.FindComponent<MessageBox>();
        await messageBox.InvokeAsync(() => messageBox.Instance.OnResult.InvokeAsync(MessageBoxResult.Yes));
        var result = await task;

        // assert
        Assert.AreEqual(MessageBoxResult.Yes, result);
    }

    [TestMethod]
    public void MultipleOnShowEvents_ShowsLatestMessageBox()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>() as IMessageBoxService;

        // act
        _ = service!.ShowAsync("First message", "First");
        _ = service.ShowAsync("Second message", "Second");

        // assert
        var messageBoxes = comp.FindComponents<MessageBox>();
        Assert.HasCount(1, messageBoxes);
        Assert.AreEqual("Second message", messageBoxes[0].Instance.Message);
        Assert.AreEqual("Second", messageBoxes[0].Instance.Title);
    }

    [TestMethod]
    public void ShowErrorAsync_RendersWithErrorType()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        _ = service!.ShowErrorAsync("Error occurred");

        // assert
        var messageBox = comp.FindComponent<MessageBox>();
        Assert.AreEqual(MessageType.Error, messageBox.Instance.Type);
        Assert.AreEqual("Error occurred", messageBox.Instance.Message);
    }

    [TestMethod]
    public void ShowWarningAsync_RendersWithWarningType()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        _ = service!.ShowWarningAsync("Warning message");

        // assert
        var messageBox = comp.FindComponent<MessageBox>();
        Assert.AreEqual(MessageType.Warning, messageBox.Instance.Type);
        Assert.AreEqual("Warning message", messageBox.Instance.Message);
    }

    [TestMethod]
    public void ShowSuccessAsync_RendersWithSuccessType()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        _ = service!.ShowSuccessAsync("Success!");

        // assert
        var messageBox = comp.FindComponent<MessageBox>();
        Assert.AreEqual(MessageType.Success, messageBox.Instance.Type);
        Assert.AreEqual("Success!", messageBox.Instance.Message);
    }

    [TestMethod]
    public void ConfirmAsync_RendersWithYesNoButtons()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        _ = service!.ConfirmAsync("Confirm?");

        // assert
        var messageBox = comp.FindComponent<MessageBox>();
        Assert.AreEqual(MessageBoxButtons.YesNo, messageBox.Instance.Buttons);
    }

    [TestMethod]
    public void Dispose_UnsubscribesFromServiceEvent()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var services = ctx.Services;
        services.AddMessageBox();
        var comp = ctx.Render<MessageBoxProvider>();
        var service = services.GetService<IMessageBoxService>();

        // act
        comp.Instance.Dispose();
        _ = service!.ShowAsync("Test message after dispose");

        // assert
        var messageBoxes = comp.FindComponents<MessageBox>();
        Assert.IsEmpty(messageBoxes);
    }
}
