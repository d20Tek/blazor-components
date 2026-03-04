using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalFormDialogEventsTests
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
}
