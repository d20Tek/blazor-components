namespace D20Tek.BlazorComponents.UnitTests.Spinner;

[TestClass]
public class ContentSpinnerTests
{
    [TestMethod]
    public void DefaultRender()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ContentSpinner>();

        // assert
        var expectedHtml = @"<div role=""status"" class=""content-spinner""></div>";
        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_IsVisibleFalse()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ContentSpinner>(parameters => parameters.Add(p => p.IsVisible, false));

        // assert
        comp.MarkupMatches(string.Empty);
    }

    [TestMethod]
    public void Render_WithChildContent()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ContentSpinner>(parameters => parameters.AddChildContent("Test message..."));

        // assert
        var expectedHtml = @"<div role=""status"" class=""content-spinner"">Test message...</div>";
        comp.MarkupMatches(expectedHtml);
    }


    [TestMethod]
    public void Render_WithImageChildContent()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ContentSpinner>(parameters => parameters
            .AddChildContent(@"<img alt=""test image"" src=""./test/image.png"" />"));

        // assert
        var expectedHtml = @"
<div role=""status"" class=""content-spinner"">
    <img alt=""test image"" src=""./test/image.png"" />
</div>";
        comp.MarkupMatches(expectedHtml);
    }

    [TestMethod]
    public void Render_WithSize()
    {
        // arrange
        var ctx = new BunitContext();

        // act
        var comp = ctx.Render<ContentSpinner>(parameters => parameters
            .Add(p => p.Size, Size.Medium)
            .AddChildContent("Test message..."));

        // assert
        var expectedHtml = @"
<div role=""status"" class=""content-spinner"" style=""--spinner-width: 4rem; --spinner-height: 4rem;"">
    Test message...
</div>";
        comp.MarkupMatches(expectedHtml);
    }
}
