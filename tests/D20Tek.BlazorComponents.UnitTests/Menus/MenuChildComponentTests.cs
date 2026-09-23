namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class MenuChildComponentTests
{
    private static BunitContext CreateContext()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx;
    }

    [TestMethod]
    public void MenuItem_WithoutParent_RendersActionWithoutError()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuItem>(p => p.Add(x => x.Label, "Standalone"));

        // Assert
        var item = comp.Find("button[role=menuitem]");
        Assert.AreEqual("Standalone", item.TextContent.Trim());
    }

    [TestMethod]
    public async Task MenuItem_WithoutParent_ClickInvokesOnClick()
    {
        // Arrange
        var ctx = CreateContext();
        var clicked = false;
        var comp = ctx.Render<MenuItem>(p => p
            .Add(x => x.Label, "Standalone")
            .Add(x => x.OnClick, EventCallback.Factory.Create(new object(), () => clicked = true)));

        // Act
        await comp.Find("button[role=menuitem]").ClickAsync(new());

        // Assert
        Assert.IsTrue(clicked);
    }

    [TestMethod]
    public void MenuItem_WithDestructiveAndDisabled_AddsBothModifierClasses()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuItem>(p => p
            .Add(x => x.Label, "Delete")
            .Add(x => x.IsDestructive, true)
            .Add(x => x.IsDisabled, true));

        // Assert
        var item = comp.Find("button[role=menuitem]");
        Assert.IsTrue(item.ClassList.Contains("d20tek-flyout-menu__item--destructive"));
        Assert.IsTrue(item.ClassList.Contains("d20tek-flyout-menu__item--disabled"));
        Assert.IsTrue(item.HasAttribute("disabled"));
    }

    [TestMethod]
    public void MenuLink_WithoutParent_RendersAnchor()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuLink>(p => p
            .Add(x => x.Label, "Docs")
            .Add(x => x.Href, "/docs"));

        // Assert
        var link = comp.Find("a[role=menuitem]");
        Assert.AreEqual("/docs", link.GetAttribute("href"));
    }

    [TestMethod]
    public void MenuLink_WithBlankTarget_AddsRelNoopener()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuLink>(p => p
            .Add(x => x.Label, "Docs")
            .Add(x => x.Href, "https://example.com")
            .Add(x => x.Target, "_blank"));

        // Assert
        Assert.AreEqual("noopener noreferrer", comp.Find("a[role=menuitem]").GetAttribute("rel"));
    }

    [TestMethod]
    public void MenuHeader_RendersGroupRoleWithLabel()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuHeader>(p => p.Add(x => x.Label, "Section"));

        // Assert
        var header = comp.Find("[role=group]");
        Assert.AreEqual("Section", header.TextContent);
        Assert.AreEqual("Section", header.GetAttribute("aria-label"));
    }

    [TestMethod]
    public void MenuSeparator_RendersSeparatorRole()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuSeparator>();

        // Assert
        Assert.IsNotNull(comp.Find("[role=separator]"));
    }

    [TestMethod]
    public async Task MenuItem_InsideMenu_CloseOnClickFalse_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p
            .AddChildContent<MenuItem>(i => i
                .Add(m => m.Label, "Stay")
                .Add(m => m.CloseOnClick, false)));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Act
        await comp.Find("button[role=menuitem]").ClickAsync(new());

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task MenuLink_InsideMenu_UsesParentWrapCloseAndClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p
            .AddChildContent<MenuLink>(i => i
                .Add(m => m.Label, "Docs")
                .Add(m => m.Href, "/docs")));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Act
        await comp.Find("a[role=menuitem]").ClickAsync(new());

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public void MenuItem_WithIconContent_RendersIconSpan()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<MenuItem>(p => p
            .Add(x => x.Label, "Edit")
            .Add(x => x.IconContent, (RenderFragment)(builder =>
                builder.AddMarkupContent(0, "<svg class=\"child-svg\"></svg>"))));

        // Assert
        Assert.IsNotNull(comp.Find("span.d20tek-flyout-menu__item-icon .child-svg"));
    }
}
