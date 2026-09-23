namespace D20Tek.BlazorComponents.UnitTests.Menus.Flyout;

[TestClass]
public class FlyoutMenuInteractionTests
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
    public async Task OnOutsideInteraction_WhenCloseOnOutsideClickTrue_ClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.CloseOnOutsideClick, true));
        await comp.Instance.OpenAsync();

        // Act
        await comp.InvokeAsync(() => comp.Instance.OnOutsideInteraction());

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OnOutsideInteraction_WhenCloseOnOutsideClickFalse_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.CloseOnOutsideClick, false));
        await comp.Instance.OpenAsync();

        // Act
        await comp.InvokeAsync(() => comp.Instance.OnOutsideInteraction());

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OnScrollInteraction_WhenCloseOnScrollTrue_ClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.CloseOnScroll, true));
        await comp.Instance.OpenAsync();

        // Act
        await comp.InvokeAsync(() => comp.Instance.OnScrollInteraction());

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OnScrollInteraction_WhenCloseOnScrollFalse_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.CloseOnScroll, false));
        await comp.Instance.OpenAsync();

        // Act
        await comp.InvokeAsync(() => comp.Instance.OnScrollInteraction());

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OpenAsync_WhenAlreadyOpen_IsNoOp()
    {
        // Arrange
        var ctx = CreateContext();
        var openCount = 0;
        var comp = ctx.Render<FlyoutMenu>(p => p
            .Add(x => x.Items, Items())
            .Add(x => x.OnOpen, EventCallback.Factory.Create(new object(), () => openCount++)));
        await comp.Instance.OpenAsync();

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.AreEqual(1, openCount);
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task CloseAsync_WhenAlreadyClosed_IsNoOp()
    {
        // Arrange
        var ctx = CreateContext();
        var closeCount = 0;
        var comp = ctx.Render<FlyoutMenu>(p => p
            .Add(x => x.Items, Items())
            .Add(x => x.OnClose, EventCallback.Factory.Create(new object(), [ExcludeFromCodeCoverage]() => closeCount++)));

        // Act
        await comp.Instance.CloseAsync();

        // Assert
        Assert.AreEqual(0, closeCount);
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OpenAsync_WithSingleOpenFalse_DoesNotCoordinateButOpens()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.SingleOpen, false));

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OpenAsync_WithOnlyHeadersAndSeparators_DoesNotOpen()
    {
        // Arrange
        var ctx = CreateContext();
        IReadOnlyList<IMenuEntry> items =
        [
            new MenuHeaderItem { Label = "Header" },
            new MenuSeparatorItem(),
        ];
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, items));

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OpenAsync_WithChildContentOnly_Opens()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.AddChildContent<MenuItem>(i => i.Add(m => m.Label, "Child")));

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task OpenAsync_WithNoItemsAndNoChildContent_DoesNotOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.TriggerAriaLabel, "Menu"));

        // Act
        await comp.Instance.OpenAsync();

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task SingleOpenGroup_OpeningSecondMenu_ClosesFirst()
    {
        // Arrange
        var ctx = CreateContext();
        var group = Guid.NewGuid().ToString();
        var first = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, group));
        var second = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, group));
        await first.Instance.OpenAsync();

        // Act
        await second.Instance.OpenAsync();

        // Assert
        Assert.IsFalse(first.Instance.IsOpen);
        Assert.IsTrue(second.Instance.IsOpen);
    }

    [TestMethod]
    public async Task DifferentGroups_BothMenusStayOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var first = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, Guid.NewGuid().ToString()));
        var second = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, Guid.NewGuid().ToString()));
        await first.Instance.OpenAsync();

        // Act
        await second.Instance.OpenAsync();

        // Assert
        Assert.IsTrue(first.Instance.IsOpen);
        Assert.IsTrue(second.Instance.IsOpen);
    }

    [TestMethod]
    public async Task DisposeAsync_WhileOpen_UnregistersFromCoordinator()
    {
        // Arrange
        var ctx = CreateContext();
        var group = Guid.NewGuid().ToString();
        var first = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, group));
        await first.Instance.OpenAsync();

        // Act
        await first.Instance.DisposeAsync();
        var second = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()).Add(x => x.GroupName, group));
        await second.Instance.OpenAsync();

        // Assert
        Assert.IsTrue(second.Instance.IsOpen);
    }
}
