namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileBehaviorTests
{
    [TestMethod]
    public void Click_WithClickedHandler_InvokesCallback()
    {
        // Arrange
        var ctx = new BunitContext();
        var clicked = false;
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .Add(t => t.Clicked, EventCallback.Factory.Create<MouseEventArgs>(this, () => clicked = true)));

        // Act
        comp.Find("button").Click();

        // Assert
        Assert.IsTrue(clicked);
    }

    [TestMethod]
    public void ImageError_FallsBackToAvatar()
    {
        // Arrange
        var ctx = new BunitContext();
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Mountain Retreat")
            .Add(t => t.ImageUrl, "https://example.com/missing.png"));

        // Act
        comp.Find("img.tile-image").TriggerEvent("onerror", new EventArgs());

        // Assert
        Assert.IsEmpty(comp.FindAll("img.tile-image"));
        Assert.AreEqual("MR", comp.Find(".tile-avatar").TextContent.Trim());
    }

    [TestMethod]
    public void Render_PassesThroughAdditionalAttributes()
    {
        // Arrange
        var ctx = new BunitContext();

        // Act
        var comp = ctx.Render<Tile>(p => p
            .Add(t => t.Title, "Hello")
            .AddUnmatched("data-test", "abc"));

        // Assert
        Assert.AreEqual("abc", comp.Find("button").GetAttribute("data-test"));
    }
}
