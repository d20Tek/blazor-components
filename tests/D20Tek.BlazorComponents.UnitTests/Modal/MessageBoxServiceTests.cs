using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxServiceTests
{
    [TestMethod]
    public void ShowAsync_RaisesOnShowEvent()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ShowAsync("Test message", "Test Title", MessageType.Information);

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Test message", capturedOptions.Message);
        Assert.AreEqual("Test Title", capturedOptions.Title);
        Assert.AreEqual(MessageType.Information, capturedOptions.Type);
        Assert.AreEqual(MessageBoxButtons.Ok, capturedOptions.Buttons);
        Assert.IsNotNull(capturedOptions.TaskCompletionSource);
    }

    [TestMethod]
    public void ShowAsync_WithDefaults_UsesDefaultTitle()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ShowAsync("Test message");

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Message", capturedOptions.Title);
        Assert.AreEqual(MessageType.Information, capturedOptions.Type);
    }

    [TestMethod]
    public void ConfirmAsync_RaisesOnShowEvent()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ConfirmAsync("Confirm this?", "Confirm", MessageType.Question, MessageBoxButtons.YesNo);

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Confirm this?", capturedOptions.Message);
        Assert.AreEqual("Confirm", capturedOptions.Title);
        Assert.AreEqual(MessageType.Question, capturedOptions.Type);
        Assert.AreEqual(MessageBoxButtons.YesNo, capturedOptions.Buttons);
    }

    [TestMethod]
    public async Task ConfirmAsync_ReturnsResult_WhenSetResultCalled()
    {
        // arrange
        var service = new MessageBoxService();
        service.OnShow += options => { };

        // act
        var task = service.ConfirmAsync("Confirm?");
        service.SetResult(MessageBoxResult.Yes);
        var result = await task;

        // assert
        Assert.AreEqual(MessageBoxResult.Yes, result);
    }

    [TestMethod]
    public void ShowErrorAsync_RaisesOnShowEvent_WithErrorType()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ShowErrorAsync("Error occurred", "Error");

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Error occurred", capturedOptions.Message);
        Assert.AreEqual("Error", capturedOptions.Title);
        Assert.AreEqual(MessageType.Error, capturedOptions.Type);
        Assert.AreEqual(MessageBoxButtons.Ok, capturedOptions.Buttons);
    }

    [TestMethod]
    public void ShowWarningAsync_RaisesOnShowEvent_WithWarningType()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ShowWarningAsync("Warning message");

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Warning message", capturedOptions.Message);
        Assert.AreEqual("Warning", capturedOptions.Title);
        Assert.AreEqual(MessageType.Warning, capturedOptions.Type);
    }

    [TestMethod]
    public void ShowSuccessAsync_RaisesOnShowEvent_WithSuccessType()
    {
        // arrange
        var service = new MessageBoxService();
        MessageBoxOptions? capturedOptions = null;
        service.OnShow += options => capturedOptions = options;

        // act
        var task = service.ShowSuccessAsync("Success!", "Success");

        // assert
        Assert.IsNotNull(capturedOptions);
        Assert.AreEqual("Success!", capturedOptions.Message);
        Assert.AreEqual("Success", capturedOptions.Title);
        Assert.AreEqual(MessageType.Success, capturedOptions.Type);
    }

    [TestMethod]
    public async Task SetResult_CompletesTaskCompletionSource()
    {
        // arrange
        var service = new MessageBoxService();
        service.OnShow += options => { };
        var task = service.ConfirmAsync("Test");

        // act
        service.SetResult(MessageBoxResult.Ok);
        var result = await task;

        // assert
        Assert.AreEqual(MessageBoxResult.Ok, result);
    }

    [TestMethod]
    public async Task MultipleCallsToSetResult_OnlyFirstCompletes()
    {
        // arrange
        var service = new MessageBoxService();
        service.OnShow += options => { };
        var task = service.ConfirmAsync("Test");

        // act
        service.SetResult(MessageBoxResult.Yes);
        service.SetResult(MessageBoxResult.No);
        var result = await task;

        // assert
        Assert.AreEqual(MessageBoxResult.Yes, result);
    }
}
