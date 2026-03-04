namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalFormDialogSizeTests
{
    [ExcludeFromCodeCoverage]
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    [TestMethod]
    public void DefaultSize_IsMedium()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model));

        // assert
        Assert.AreEqual(Size.Medium, comp.Instance.Size);
    }

    [TestMethod]
    public void Render_WithSizeSmall()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.Size, Size.Small));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-sm"));
    }

    [TestMethod]
    public void Render_WithSizeLarge()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.Size, Size.Large));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-lg"));
    }
}
