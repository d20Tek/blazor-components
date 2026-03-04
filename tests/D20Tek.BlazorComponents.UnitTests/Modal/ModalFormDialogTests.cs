namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalFormDialogTests
{
    [ExcludeFromCodeCoverage]
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <form>
        <div class=""modal-dialog__body""></div>
        <footer class=""modal-dialog__footer"">
            <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
            <button type=""submit"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
        </footer>
    </form>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithTitleAndSummary()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.Title, "Test Title")
                      .Add(p => p.Summary, "Test summary."));

        // assert
        var header = comp.Find(".modal-dialog__header-content");
        Assert.IsTrue(header.InnerHtml.Contains("Test Title"));
        Assert.IsTrue(header.InnerHtml.Contains("Test summary."));
    }

    [TestMethod]
    public void Render_WithCustomButtonText()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.SubmitButtonText, "Save")
                      .Add(p => p.CancelButtonText, "Dismiss"));

        // assert
        var submitBtn = comp.Find(".modal-dialog__btn-submit");
        var cancelBtn = comp.Find(".modal-dialog__btn-cancel");
        Assert.AreEqual("Save", submitBtn.TextContent.Trim());
        Assert.AreEqual("Dismiss", cancelBtn.TextContent.Trim());
    }

    [TestMethod]
    public void Render_WithShowCancelButton_False()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.ShowCancelButton, false));

        // assert
        var cancelButtons = comp.FindAll(".modal-dialog__btn-cancel");
        Assert.AreEqual(0, cancelButtons.Count);
    }

    [TestMethod]
    public void Render_WithShowCloseButton_False()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.ShowCloseButton, false));

        // assert
        var closeButtons = comp.FindAll(".modal-dialog__close-btn");
        Assert.AreEqual(0, closeButtons.Count);
    }

    [TestMethod]
    public void SubmitButton_IsTypeSubmit()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model));

        // assert
        var submitBtn = comp.Find(".modal-dialog__btn-submit");
        Assert.AreEqual("submit", submitBtn.GetAttribute("type"));
    }

    [TestMethod]
    public void Render_IsNotVisible_RendersNothing()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.IsVisible, false));

        // assert
        comp.MarkupMatches(string.Empty);
    }
}
