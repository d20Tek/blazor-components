namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class MessageBoxPositionTests
{
    [TestMethod]
    public void Render_WithPositionTop()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message")
                      .Add(p => p.Position, VerticalPosition.Top));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--top"));
        Assert.AreEqual(VerticalPosition.Top, comp.Instance.Position);
    }

    [TestMethod]
    public void Render_WithPositionCenter()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message")
                      .Add(p => p.Position, VerticalPosition.Center));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--center"));
        Assert.AreEqual(VerticalPosition.Center, comp.Instance.Position);
    }

    [TestMethod]
    public void Render_WithPositionBottom()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        // act
        var comp = ctx.Render<MessageBox>(parameters =>
            parameters.Add(p => p.Message, "Test message")
                      .Add(p => p.Position, VerticalPosition.Bottom));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--bottom"));
        Assert.AreEqual(VerticalPosition.Bottom, comp.Instance.Position);
    }
}
