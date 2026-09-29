namespace D20Tek.BlazorComponents.UnitTests.Menus.Flyout;

[TestClass]
public class FlyoutMenuTests
{
    private static BunitContext CreateContext()
    {
        var ctx = new BunitContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        return ctx;
    }

    private static IReadOnlyList<IMenuEntry> SampleItems(Action? onClick = null) =>
    [
        new MenuHeaderItem { Label = "Actions" },
        new MenuActionItem
        {
            Label = "Edit",
            OnClick = EventCallback.Factory.Create(new object(), [ExcludeFromCodeCoverage]() => onClick?.Invoke())
        },
        new MenuSeparatorItem(),
        new MenuActionItem { Label = "Delete", IsDestructive = true },
    ];

    [TestMethod]
    public void Render_ByDefault_ShowsTriggerButAndNoPopup()
    {
        // Arrange
        var ctx = CreateContext();

        // Act
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Assert
        Assert.IsNotNull(comp.Find(".d20tek-flyout-menu__trigger__button"));
        Assert.IsEmpty(comp.FindAll("[role=menu]"));
    }

    [TestMethod]
    public async Task Click_OnTrigger_OpensPopupWithMenuRole()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsNotNull(comp.Find("[role=menu]"));
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task Click_Twice_TogglesPopupClosed()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
        Assert.IsEmpty(comp.FindAll("[role=menu]"));
    }

    [TestMethod]
    public async Task Open_RendersActionItemsWithMenuItemRole()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var items = comp.FindAll("[role=menuitem]");
        Assert.HasCount(2, items);
    }

    [TestMethod]
    public async Task ActionItem_Click_InvokesCallbackAndClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var clicked = false;
        var comp = ctx.Render<FlyoutMenu>(
            p => p.Add(x => x.Items, SampleItems(() => clicked = true)));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Act
        await comp.FindAll("[role=menuitem]")[0].ClickAsync(new());

        // Assert
        Assert.IsTrue(clicked);
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task DestructiveItem_HasDestructiveClass()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var destructive = comp.Find(".d20tek-flyout-menu__item--destructive");
        Assert.IsNotNull(destructive);
    }

    [TestMethod]
    public async Task DisabledItem_HasAriaDisabledAndNoClickHandler()
    {
        // Arrange
        var ctx = CreateContext();
        var clicked = false;
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuActionItem
            {
                Label = "Cut",
                IsDisabled = true,
                OnClick = EventCallback.Factory.Create(new object(), [ExcludeFromCodeCoverage]() => clicked = true)
            },
        ];
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, items));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Act
        var item = comp.Find("[role=menuitem]");

        // Assert
        Assert.AreEqual("true", item.GetAttribute("aria-disabled"));
        Assert.IsFalse(clicked);
    }

    [TestMethod]
    public async Task Header_RendersWithGroupRole()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var header = comp.Find("[role=group]");
        Assert.AreEqual("Actions", header.TextContent);
    }

    [TestMethod]
    public async Task Separator_RendersWithSeparatorRole()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, SampleItems()));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsNotNull(comp.Find("[role=separator]"));
    }

    [TestMethod]
    public async Task LinkItem_WithBlankTarget_AddsRelNoopener()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuLinkItem { Label = "Docs", Href = "https://example.com", Target = "_blank" },
        ];
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, items));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        var link = comp.Find("a[role=menuitem]");
        Assert.AreEqual("noopener noreferrer", link.GetAttribute("rel"));
        Assert.AreEqual("https://example.com", link.GetAttribute("href"));
    }

    [TestMethod]
    public async Task EmptyMenu_DoesNotOpen()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items = [];
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, items));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task Disabled_Menu_DoesNotOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(
            p => p.Add(x => x.Items, SampleItems())
                  .Add(x => x.Disabled, true));

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task Trigger_ExposesAriaAttributes()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(
            p => p.Add(x => x.Items, SampleItems()).Add(x => x.Id, "my-menu"));

        // Act
        var button = comp.Find(".d20tek-flyout-menu__trigger__button");

        // Assert
        Assert.AreEqual("menu", button.GetAttribute("aria-haspopup"));
        Assert.AreEqual("false", button.GetAttribute("aria-expanded"));
        Assert.AreEqual("my-menu", button.GetAttribute("aria-controls"));
    }

    [TestMethod]
    public async Task Open_WithExplicitId_UsesIdForPopup()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(
            p => p.Add(x => x.Items, SampleItems()).Add(x => x.Id, "my-menu"));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.AreEqual("my-menu", comp.Find("[role=menu]").GetAttribute("id"));
    }

    [TestMethod]
    public async Task CustomTrigger_ReflectsOpenState()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p
            .Add(x => x.Items, SampleItems())
            .Add(x => x.Trigger, (RenderFragment<FlyoutMenuContext>)(context => builder =>
            {
                builder.OpenElement(0, "button");
                builder.AddAttribute(1, "class", "custom-trigger");
                builder.AddAttribute(2, "onclick", context.Toggle);
                builder.AddContent(3, context.IsOpen ? "open" : "closed");
                builder.CloseElement();
            })));

        // Act
        await comp.Find(".custom-trigger").ClickAsync(new());

        // Assert
        Assert.AreEqual("open", comp.Find(".custom-trigger").TextContent);
    }

    [TestMethod]
    public async Task OnOpenAndOnClose_Callbacks_Fire()
    {
        // Arrange
        var ctx = CreateContext();
        var opened = false;
        var closed = false;
        var comp = ctx.Render<FlyoutMenu>(p => p
            .Add(x => x.Items, SampleItems())
            .Add(x => x.OnOpen, EventCallback.Factory.Create(new object(), () => opened = true))
            .Add(x => x.OnClose, EventCallback.Factory.Create(new object(), () => closed = true)));

        // Act
        await comp.Instance.OpenAsync();
        await comp.Instance.CloseAsync();

        // Assert
        Assert.IsTrue(opened);
        Assert.IsTrue(closed);
    }

    [TestMethod]
    public async Task ChildContent_Templated_RendersItems()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p
            .AddChildContent<MenuItem>(item => item.Add(i => i.Label, "Templated")));

        // Act
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());

        // Assert
        Assert.AreEqual("Templated", comp.Find(".d20tek-flyout-menu__item-label").TextContent);
    }
}
