using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxButtonTests
{
    [TestMethod]
    public void Render_OkButton()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.Ok));

        // assert
        var buttons = comp.FindAll(".modal-dialog__footer button");
        Assert.HasCount(1, buttons);
        Assert.AreEqual("OK", buttons[0].TextContent);
        Assert.IsTrue(buttons[0].ClassList.Contains("modal-dialog__btn-submit"));
    }

    [TestMethod]
    public void Render_OkCancelButtons()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.OkCancel));

        // assert
        var buttons = comp.FindAll(".modal-dialog__footer button");
        Assert.HasCount(2, buttons);
        Assert.AreEqual("Cancel", buttons[0].TextContent);
        Assert.AreEqual("OK", buttons[1].TextContent);
    }

    [TestMethod]
    public void Render_YesNoButtons()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNo));

        // assert
        var buttons = comp.FindAll(".modal-dialog__footer button");
        Assert.HasCount(2, buttons);
        Assert.AreEqual("No", buttons[0].TextContent);
        Assert.AreEqual("Yes", buttons[1].TextContent);
    }

    [TestMethod]
    public void Render_YesNoCancelButtons()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNoCancel));

        // assert
        var buttons = comp.FindAll(".modal-dialog__footer button");
        Assert.HasCount(3, buttons);
        Assert.AreEqual("Cancel", buttons[0].TextContent);
        Assert.AreEqual("No", buttons[1].TextContent);
        Assert.AreEqual("Yes", buttons[2].TextContent);
    }

    [TestMethod]
    public async Task OkButton_ClickFiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.Ok)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var okButton = comp.Find(".modal-dialog__btn-submit");
        await okButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Ok, result);
    }

    [TestMethod]
    public async Task OkCancelButtons_OkClick_FiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.OkCancel)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var okButton = comp.Find(".modal-dialog__btn-submit");
        await okButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Ok, result);
    }

    [TestMethod]
    public async Task OkCancelButtons_CancelClick_FiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.OkCancel)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var cancelButton = comp.Find(".modal-dialog__btn-cancel");
        await cancelButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Cancel, result);
    }

    [TestMethod]
    public async Task YesButton_ClickFiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNo)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var yesButton = comp.Find(".modal-dialog__btn-submit");
        await yesButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Yes, result);
    }

    [TestMethod]
    public async Task NoButton_ClickFiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNo)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var noButton = comp.Find(".modal-dialog__btn-cancel");
        await noButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.No, result);
    }

    [TestMethod]
    public async Task CloseButton_ClickFiresOnResult_WithNone()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.OnResult, value => result = value));

        // act
        var closeButton = comp.Find(".modal-dialog__close-btn");
        await closeButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.None, result);
    }

    [TestMethod]
    public void YesNoCancelButtons_HasCorrectCssClasses()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNoCancel));

        // assert
        var buttons = comp.FindAll(".modal-dialog__footer button");
        Assert.IsTrue(buttons[0].ClassList.Contains("modal-dialog__btn-cancel"));
        Assert.IsTrue(buttons[1].ClassList.Contains("modal-dialog__btn-secondary"));
        Assert.IsTrue(buttons[2].ClassList.Contains("modal-dialog__btn-submit"));
    }

    [TestMethod]
    public async Task YesNoCancelButtons_YesClick_FiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNoCancel)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var yesButton = comp.Find(".modal-dialog__btn-submit");
        await yesButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Yes, result);
    }

    [TestMethod]
    public async Task YesNoCancelButtons_NoClick_FiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNoCancel)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var noButton = comp.Find(".modal-dialog__btn-secondary");
        await noButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.No, result);
    }

    [TestMethod]
    public async Task YesNoCancelButtons_CancelClick_FiresOnResult()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        MessageBoxResult? result = null;

        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test")
                      .Add(p => p.Buttons, MessageBoxButtons.YesNoCancel)
                      .Add(p => p.OnResult, value => result = value));

        // act
        var cancelButton = comp.Find(".modal-dialog__btn-cancel");
        await cancelButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.AreEqual(MessageBoxResult.Cancel, result);
    }
}
