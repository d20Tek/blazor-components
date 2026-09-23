namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class FlyoutMenuRenderingTests
{
    private static BunitContext CreateContext()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx;
    }

    private static IReadOnlyList<IMenuEntry> Items() =>
    [
        new MenuActionItem { Label = "One" },
    ];

    [TestMethod]
    public void Render_WithIsVisibleFalse_RendersNothing()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.IsVisible, false));

        // Assert
        Assert.AreEqual(0, comp.Markup.Trim().Length);
    }

    [TestMethod]
    public void Render_WithTriggerIcon_RendersIconElementNotSvg()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.TriggerIcon, "oi oi-menu"));

        // Assert
        Assert.IsNotNull(comp.Find("i.oi.oi-menu"));
        Assert.AreEqual(0, comp.FindAll("svg").Count);
    }

    [TestMethod]
    public void Render_WithoutTriggerIcon_RendersDefaultSvgKebab()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()));

        // Assert
        Assert.IsNotNull(comp.Find("svg"));
    }

    [TestMethod]
    public void Render_WithTriggerCssClass_AddsClassToButton()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.TriggerCssClass, "my-trigger"));

        // Assert
        Assert.IsTrue(comp.Find(".d20tek-flyout-menu__trigger__button").ClassList.Contains("my-trigger"));
    }

    [TestMethod]
    public async Task Open_WithMenuCssClass_AddsClassToPopup()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.MenuCssClass, "my-popup"));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsTrue(comp.Find("[role=menu]").ClassList.Contains("my-popup"));
    }

    [TestMethod]
    public async Task Open_WithAnimateTrue_AddsAnimateClass()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.Animate, true));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsTrue(comp.Find("[role=menu]").ClassList.Contains("d20tek-flyout-menu__popup--animate"));
    }

    [TestMethod]
    public async Task Open_WithAnimateFalse_OmitsAnimateClass()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.Animate, false));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsFalse(comp.Find("[role=menu]").ClassList.Contains("d20tek-flyout-menu__popup--animate"));
    }

    [TestMethod]
    public async Task Open_WithZIndex_SetsZIndexCustomProperty()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.ZIndex, 5000));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var style = comp.Find("[role=menu]").GetAttribute("style");
        StringAssert.Contains(style, "--d20tek-menu-z-index");
        StringAssert.Contains(style, "5000");
    }

    [TestMethod]
    public async Task Open_WithoutZIndex_HasNoInlineStyle()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var style = comp.Find("[role=menu]").GetAttribute("style");
        Assert.IsTrue(string.IsNullOrEmpty(style));
    }

    [TestMethod]
    public void Render_WithSizeNone_OmitsSizeClass()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.Size, Size.None));

        // Assert
        var root = comp.Find(".d20tek-flyout-menu");
        Assert.IsFalse(root.ClassList.Contains("d20tek-flyout-menu--md"));
    }

    [TestMethod]
    public void Render_WithSizeLarge_AddsSizeClass()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.Size, Size.Large));

        // Assert
        Assert.IsTrue(comp.Find(".d20tek-flyout-menu").ClassList.Contains("d20tek-flyout-menu--lg"));
    }

    [TestMethod]
    public async Task TriggerButton_WhenOpen_HasIsOpenClassAndAriaExpandedTrue()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var button = comp.Find(".d20tek-flyout-menu__trigger__button");
        Assert.IsTrue(button.ClassList.Contains("is-open"));
        Assert.AreEqual("true", button.GetAttribute("aria-expanded"));
    }

    [TestMethod]
    public void Render_WithRemainingAttributes_ForwardsToRootElement()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p
            .Add(x => x.Items, Items())
            .AddUnmatched("data-testid", "menu-root"));

        // Assert
        Assert.AreEqual("menu-root", comp.Find(".d20tek-flyout-menu").GetAttribute("data-testid"));
    }

    [TestMethod]
    public void Render_WithDisabledTrue_TriggerButtonIsDisabled()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.Disabled, true));

        // Assert
        Assert.IsTrue(comp.Find(".d20tek-flyout-menu__trigger__button").HasAttribute("disabled"));
    }
}
