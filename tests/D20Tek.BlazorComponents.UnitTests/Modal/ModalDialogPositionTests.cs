namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalDialogPositionTests
{
    [TestMethod]
    public void DefaultPosition_IsCenter()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>();

        // assert
        Assert.AreEqual(VerticalPosition.Center, comp.Instance.Position);
    }

    [TestMethod]
    public void Render_WithPositionCenter()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Position, VerticalPosition.Center));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--center"));
    }

    [TestMethod]
    public void Render_WithPositionTop()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Position, VerticalPosition.Top));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--top"));
    }

    [TestMethod]
    public void Render_WithPositionBottom()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Position, VerticalPosition.Bottom));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--bottom"));
    }

    [TestMethod]
    public void Render_PositionAndSize_BothApplied()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Position, VerticalPosition.Top)
                      .Add(p => p.Size, Size.Large));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--top"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-lg"));
    }
}
