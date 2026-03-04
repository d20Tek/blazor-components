using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalFormDialogTests
{
    [ExcludeFromCodeCoverage]
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    private class RequiredTestModel
    {
        [Required]
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

    [TestMethod]
    public async Task OnCancelCallback_FiresOnCancelButtonClick()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var cancelFired = false;
        var model = new TestModel();

        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.OnCancel, () => { cancelFired = true; }));

        // act
        var cancelButton = comp.Find(".modal-dialog__btn-cancel");
        await cancelButton.ClickAsync(new MouseEventArgs());

        // assert
        Assert.IsTrue(cancelFired);
    }

    [TestMethod]
    public async Task OnCancelCallback_FiresOnCloseButtonClick()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var cancelFired = false;
        var model = new TestModel();

        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.OnCancel, () => { cancelFired = true; }));

        // act
        var closeButton = comp.Find(".modal-dialog__close-btn");
        await closeButton.ClickAsync(new MouseEventArgs());

        // assert
        Assert.IsTrue(cancelFired);
    }

    [TestMethod]
    public async Task OnValidSubmit_FiresOnValidFormSubmit()
    {
        // arrange
        var ctx = new BunitContext();
        EditContext? capturedContext = null;
        var model = new TestModel { Name = "Valid Name" };

        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.OnValidSubmit, (EditContext ec) => { capturedContext = ec; }));

        // act
        var form = comp.Find("form");
        await form.SubmitAsync();

        // assert
        Assert.IsNotNull(capturedContext);
    }

    [TestMethod]
    public async Task OnInvalidSubmit_FiresOnInvalidFormSubmit()
    {
        // arrange
        var ctx = new BunitContext();
        EditContext? capturedContext = null;
        var model = new RequiredTestModel(); // Name is empty, violating [Required]

        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model)
                      .Add(p => p.ChildContent, (RenderTreeBuilder builder) =>
                      {
                          builder.OpenComponent<DataAnnotationsValidator>(0);
                          builder.CloseComponent();
                      })
                      .Add(p => p.OnInvalidSubmit, (EditContext ec) => { capturedContext = ec; }));

        // act
        var form = comp.Find("form");
        await form.SubmitAsync();

        // assert
        Assert.IsNotNull(capturedContext);
    }

    [TestMethod]
    public void DefaultButtonProperties()
    {
        // arrange
        var ctx = new BunitContext();
        var model = new TestModel();

        // act
        var comp = ctx.Render<ModalFormDialog>(parameters =>
            parameters.Add(p => p.Model, model));

        // assert
        Assert.AreEqual("Submit", comp.Instance.SubmitButtonText);
        Assert.AreEqual("Cancel", comp.Instance.CancelButtonText);
        Assert.IsTrue(comp.Instance.ShowCancelButton);
        Assert.IsTrue(comp.Instance.ShowCloseButton);
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
