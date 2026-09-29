namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class MenuMarkupTests
{
    private static BunitContext CreateContext()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx;
    }

    private static async Task<IRenderedComponent<FlyoutMenu>> OpenWithItemsAsync(
        BunitContext ctx, IReadOnlyList<IMenuEntry> items)
    {
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, items));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());
        return comp;
    }

    [TestMethod]
    public async Task ActionItem_WithIconString_RendersIconElement()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem { Label = "Edit", Icon = "oi oi-pencil" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        var icon = comp.Find("i.d20tek-flyout-menu__item-icon");
        Assert.IsTrue(icon.ClassList.Contains("oi"));
        Assert.IsTrue(icon.ClassList.Contains("oi-pencil"));
    }

    [TestMethod]
    public async Task ActionItem_WithIconContent_RendersSpanIconWithContent()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem
            {
                Label = "Edit",
                IconContent = builder => builder.AddMarkupContent(0, "<svg class=\"custom-svg\"></svg>")
            },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        var iconSpan = comp.Find("span.d20tek-flyout-menu__item-icon");
        Assert.AreEqual("true", iconSpan.GetAttribute("aria-hidden"));
        Assert.IsNotNull(comp.Find(".custom-svg"));
    }

    [TestMethod]
    public async Task ActionItem_WithoutIcon_RendersNoIconElement()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem { Label = "Edit" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        Assert.IsEmpty(comp.FindAll(".d20tek-flyout-menu__item-icon"));
    }

    [TestMethod]
    public async Task ActionItem_WithAriaLabel_SetsAriaLabelAttribute()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem { Label = "Edit", AriaLabel = "Edit entry" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        Assert.AreEqual("Edit entry", comp.Find("[role=menuitem]").GetAttribute("aria-label"));
    }

    [TestMethod]
    public async Task ActionItem_IconContentTakesPrecedenceOverIconString()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem
            {
                Label = "Edit",
                Icon = "oi oi-pencil",
                IconContent = builder => builder.AddMarkupContent(0, "<svg class=\"custom-svg\"></svg>")
            },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        Assert.IsNotNull(comp.Find("span.d20tek-flyout-menu__item-icon .custom-svg"));
        Assert.IsEmpty(comp.FindAll("i.d20tek-flyout-menu__item-icon"));
    }

    [TestMethod]
    public async Task ActionItem_WithCloseOnClickFalse_KeepsMenuOpenAfterClick()
    {
        // Arrange
        var ctx = CreateContext();
        var clicked = false;
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem
            {
                Label = "Edit",
                CloseOnClick = false,
                OnClick = EventCallback.Factory.Create(new object(), () => clicked = true)
            },
        ];
        var comp = await OpenWithItemsAsync(ctx, items);

        // Act
        await comp.Find("[role=menuitem]").ClickAsync(new());

        // Assert
        Assert.IsTrue(clicked);
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task LinkItem_Disabled_HasAriaDisabledAndNoHref()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "https://example.com", IsDisabled = true },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        var link = comp.Find("a[role=menuitem]");
        Assert.AreEqual("true", link.GetAttribute("aria-disabled"));
        Assert.IsFalse(link.HasAttribute("href"));
    }

    [TestMethod]
    public async Task LinkItem_WithNonBlankTarget_DoesNotAddRel()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "/docs", Target = "_self" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        var link = comp.Find("a[role=menuitem]");
        Assert.AreEqual("_self", link.GetAttribute("target"));
        Assert.IsFalse(link.HasAttribute("rel"));
    }

    [TestMethod]
    public async Task LinkItem_WithoutTarget_HasNoTargetOrRel()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "/docs" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        var link = comp.Find("a[role=menuitem]");
        Assert.IsFalse(link.HasAttribute("target"));
        Assert.IsFalse(link.HasAttribute("rel"));
        Assert.AreEqual("/docs", link.GetAttribute("href"));
    }

    [TestMethod]
    public async Task LinkItem_WithAriaLabel_SetsAriaLabel()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "/docs", AriaLabel = "Documentation" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        Assert.AreEqual("Documentation", comp.Find("a[role=menuitem]").GetAttribute("aria-label"));
    }

    [TestMethod]
    public async Task LinkItem_WithIconString_RendersIcon()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "/docs", Icon = "oi oi-book" },
        ];

        // Act
        var comp = await OpenWithItemsAsync(ctx, items);

        // Assert
        Assert.IsTrue(comp.Find("i.d20tek-flyout-menu__item-icon").ClassList.Contains("oi-book"));
    }
}
