namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxIconTests
{
    [TestMethod]
    public void Render_InformationIcon()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Info")
                      .Add(p => p.Type, MessageType.Information));

        // assert
        var icon = comp.Find(".message-box__icon svg");
        Assert.IsNotNull(icon);
        Assert.IsTrue(icon.ClassList.Contains("message-box__icon--info"));
        Assert.AreEqual(MessageType.Information, comp.Instance.Type);
    }

    [TestMethod]
    public void Render_SuccessIcon()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Success")
                      .Add(p => p.Type, MessageType.Success));

        // assert
        var icon = comp.Find(".message-box__icon svg");
        Assert.IsNotNull(icon);
        Assert.IsTrue(icon.ClassList.Contains("message-box__icon--success"));
        Assert.AreEqual(MessageType.Success, comp.Instance.Type);
    }

    [TestMethod]
    public void Render_WarningIcon()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Warning")
                      .Add(p => p.Type, MessageType.Warning));

        // assert
        var icon = comp.Find(".message-box__icon svg");
        Assert.IsNotNull(icon);
        Assert.IsTrue(icon.ClassList.Contains("message-box__icon--warning"));
        Assert.AreEqual(MessageType.Warning, comp.Instance.Type);
    }

    [TestMethod]
    public void Render_ErrorIcon()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Error")
                      .Add(p => p.Type, MessageType.Error));

        // assert
        var icon = comp.Find(".message-box__icon svg");
        Assert.IsNotNull(icon);
        Assert.IsTrue(icon.ClassList.Contains("message-box__icon--error"));
        Assert.AreEqual(MessageType.Error, comp.Instance.Type);
    }

    [TestMethod]
    public void Render_QuestionIcon()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Question")
                      .Add(p => p.Type, MessageType.Question));

        // assert
        var icon = comp.Find(".message-box__icon svg");
        Assert.IsNotNull(icon);
        Assert.IsTrue(icon.ClassList.Contains("message-box__icon--question"));
        Assert.AreEqual(MessageType.Question, comp.Instance.Type);
    }

    [TestMethod]
    public void Render_InformationIcon_HasCircle()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Info")
                      .Add(p => p.Type, MessageType.Information));

        // assert
        var circle = comp.Find(".message-box__icon svg circle");
        Assert.IsNotNull(circle);
        Assert.AreEqual("#3b82f6", circle.GetAttribute("fill"));
    }

    [TestMethod]
    public void Render_SuccessIcon_HasCheckmark()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Success")
                      .Add(p => p.Type, MessageType.Success));

        // assert
        var circle = comp.Find(".message-box__icon svg circle");
        Assert.IsNotNull(circle);
        Assert.AreEqual("#22c55e", circle.GetAttribute("fill"));
    }

    [TestMethod]
    public void Render_ErrorIcon_HasRedCircle()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Error")
                      .Add(p => p.Type, MessageType.Error));

        // assert
        var circle = comp.Find(".message-box__icon svg circle");
        Assert.IsNotNull(circle);
        Assert.AreEqual("#ef4444", circle.GetAttribute("fill"));
    }
}
