namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalDialogSizeTests
{
    [TestMethod]
    public void DefaultSize_IsMedium()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>();

        // assert
        Assert.AreEqual(Size.Medium, comp.Instance.Size);
    }

    [TestMethod]
    public void Render_WithSizeSmall()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.Small));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-sm"));
    }

    [TestMethod]
    public void Render_WithSizeMedium()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.Medium));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-md"));
    }

    [TestMethod]
    public void Render_WithSizeLarge()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.Large));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-lg"));
    }

    [TestMethod]
    public void Render_WithSizeExtraLarge()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.ExtraLarge));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-xl"));
    }

    [TestMethod]
    public void Render_WithSizeExtraSmall()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.ExtraSmall));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-xs"));
    }

    [TestMethod]
    public void Render_WithSizeNone_NoSizeClass()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.Size, Size.None));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsFalse(dialog.ClassList.Contains("modal-dialog-xs"));
        Assert.IsFalse(dialog.ClassList.Contains("modal-dialog-sm"));
        Assert.IsFalse(dialog.ClassList.Contains("modal-dialog-md"));
        Assert.IsFalse(dialog.ClassList.Contains("modal-dialog-lg"));
        Assert.IsFalse(dialog.ClassList.Contains("modal-dialog-xl"));
    }
}
