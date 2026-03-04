namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalFormDialogPositionTests
{
    [ExcludeFromCodeCoverage]
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    [TestMethod]
    public void DefaultPosition_IsCenter()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model));

        // assert
        Assert.AreEqual(VerticalPosition.Center, comp.Instance.Position);
    }

    [TestMethod]
    public void Render_WithPositionTop()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.Position, VerticalPosition.Top));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog--top"));
    }
}
