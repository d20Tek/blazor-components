using System.Threading.Tasks;

namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalDialogEventsTests
{
    [TestMethod]
    public async Task OnCancelCallback_FiresOnCancelButtonClick()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var cancelFired = false;

        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.OnCancel, () => { cancelFired = true; }));

        // act
        var cancelButton = comp.Find(".modal-dialog__btn-cancel");
        await cancelButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.IsTrue(cancelFired);
    }

    [TestMethod]
    public async Task OnSubmitCallback_FiresOnSubmitButtonClick()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var submitFired = false;

        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.OnSubmit, () => { submitFired = true; }));

        // act
        var submitButton = comp.Find(".modal-dialog__btn-submit");
        await submitButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.IsTrue(submitFired);
    }

    [TestMethod]
    public async Task OnCloseCallback_FiresOnCloseButtonClick()
    {
        // arrange
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var closeFired = false;

        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.OnClose, () => { closeFired = true; }));

        // act
        var closeButton = comp.Find(".modal-dialog__close-btn");
        await closeButton.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        // assert
        Assert.IsTrue(closeFired);
    }

    [TestMethod]
    public void DefaultButtonProperties()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>();

        // assert
        Assert.AreEqual("Submit", comp.Instance.SubmitButtonText);
        Assert.AreEqual("Cancel", comp.Instance.CancelButtonText);
        Assert.IsTrue(comp.Instance.ShowSubmitButton);
        Assert.IsTrue(comp.Instance.ShowCancelButton);
        Assert.IsTrue(comp.Instance.ShowCloseButton);
    }
}
