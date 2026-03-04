namespace D20Tek.BlazorComponents.UnitTests.Modal;

[TestClass]
public class ModalDialogTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>();

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithTitleAndSummary()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Title, "Test Title")
                      .Add(p => p.Summary, "This is a test summary"));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title"">Test Title</h2>
            <p class=""modal-dialog__summary"">This is a test summary</p>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
        Assert.AreEqual("Test Title", comp.Instance.Title);
        Assert.AreEqual("This is a test summary", comp.Instance.Summary);
    }

    [TestMethod]
    public void Render_WithChildContent()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.Title, "Confirm")
                      .AddChildContent("<p>Are you sure you want to continue?</p>"));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title"">Confirm</h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body"">
        <p>Are you sure you want to continue?</p>
    </div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithCustomButtonText()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.CancelButtonText, "No")
                      .Add(p => p.SubmitButtonText, "Yes"));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">No</button>
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Yes</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
        Assert.AreEqual("No", comp.Instance.CancelButtonText);
        Assert.AreEqual("Yes", comp.Instance.SubmitButtonText);
    }

    [TestMethod]
    public void Render_IsVisibleFalse()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.IsVisible, false));

        // assert
        comp.MarkupMatches(string.Empty);
    }

    [TestMethod]
    public void Render_HideCloseButton()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.ShowCloseButton, false));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_HideCancelButton()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.ShowCancelButton, false));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-submit"">Submit</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_HideSubmitButton()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.ShowSubmitButton, false));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
    <footer class=""modal-dialog__footer"">
        <button type=""button"" class=""modal-dialog__btn modal-dialog__btn-cancel"">Cancel</button>
    </footer>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_HideBothButtons()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ModalDialog>(parameters =>
            parameters.Add(p => p.ShowCancelButton, false)
                      .Add(p => p.ShowSubmitButton, false));

        // assert
        var expectedHtml = @"
<dialog class=""modal-dialog modal-dialog-md modal-dialog--center"">
    <header class=""modal-dialog__header"">
        <div class=""modal-dialog__header-content"">
            <h2 class=""modal-dialog__title""></h2>
        </div>
        <button type=""button"" class=""modal-dialog__close-btn"" aria-label=""Close"">&times;</button>
    </header>
    <div class=""modal-dialog__body""></div>
</dialog>";

        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithAttributeSplat()
    {
        // arrange
        var ctx = new BunitContext();
        var attr = new Dictionary<string, object>
        {
            { "style", "border: 2px solid blue" },
            { "data-testid", "my-modal" }
        };

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.RemainingAttributes, attr));

        // assert
        var dialog = comp.Find("dialog");
        Assert.AreEqual("my-modal", dialog.GetAttribute("data-testid"));
        Assert.IsTrue(dialog.GetAttribute("style")!.Contains("border: 2px solid blue"));
    }

    [TestMethod]
    public void Render_WithAdditionalCssClasses()
    {
        // arrange
        var ctx = new BunitContext();
        var attr = new Dictionary<string, object>
        {
            { "class", "custom-modal themed" }
        };

        // act
        var comp = ctx.Render<ModalDialog>(parameters => parameters.Add(p => p.RemainingAttributes, attr));

        // assert
        var dialog = comp.Find("dialog");
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog"));
        Assert.IsTrue(dialog.ClassList.Contains("modal-dialog-md"));
        Assert.IsTrue(dialog.ClassList.Contains("custom-modal"));
        Assert.IsTrue(dialog.ClassList.Contains("themed"));
    }
}
