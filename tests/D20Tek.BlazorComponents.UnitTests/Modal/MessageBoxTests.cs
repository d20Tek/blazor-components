namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters => 
            parameters.Add(p => p.Message, "Test message"));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsNotNull(dialog);
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-sm"));
    }

    [TestMethod]
    public void Render_WithTitle()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Title, "Test Title")
                      .Add(p => p.Message, "Test message"));

        // assert
        var title = comp.Find(".modal-dialog__title");
        Assert.AreEqual("Test Title", title.TextContent);
        Assert.AreEqual("Test Title", comp.Instance.Title);
    }

    [TestMethod]
    public void Render_WithMessage()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "This is a test message"));

        // assert
        var message = comp.Find(".message-box__message");
        Assert.AreEqual("This is a test message", message.TextContent);
        Assert.AreEqual("This is a test message", comp.Instance.Message);
    }

    [TestMethod]
    public void Render_HasCloseButton()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message"));

        // assert
        var closeButton = comp.Find(".modal-dialog__close-btn");
        Assert.IsNotNull(closeButton);
        Assert.IsNotEmpty(closeButton.TextContent.Trim());
    }

    [TestMethod]
    public void Render_HasIconContainer()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message"));

        // assert
        var icon = comp.Find(".message-box__icon");
        Assert.IsNotNull(icon);
    }

    [TestMethod]
    public void Render_HasBodyWithFlexLayout()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message"));

        // assert
        var body = comp.Find(".message-box__body");
        Assert.IsNotNull(body);
        Assert.IsTrue(body.ClassList.Contains("modal-dialog__body"));
    }

    [TestMethod]
    public void Render_HasFooter()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message"));

        // assert
        var footer = comp.Find(".modal-dialog__footer");
        Assert.IsNotNull(footer);
    }
}
