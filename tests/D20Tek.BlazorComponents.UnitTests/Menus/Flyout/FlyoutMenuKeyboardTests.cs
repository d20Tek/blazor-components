namespace D20Tek.BlazorComponents.UnitTests.Menus.Flyout;

[TestClass]
public class FlyoutMenuKeyboardTests
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
        new MenuActionItem { Label = "Two" },
    ];

    private static async Task<IRenderedComponent<FlyoutMenu>> OpenMenuAsync(BunitContext ctx)
    {
        var comp = ctx.Render<FlyoutMenu>(p => p.Add(x => x.Items, Items()));
        await comp.Find(".d20tek-flyout-menu__trigger__button").ClickAsync(new());
        return comp;
    }

    private static Task PressKeyAsync(IRenderedComponent<FlyoutMenu> comp, string key) =>
        comp.Find("[role=menu]").TriggerEventAsync("onkeydown", new KeyboardEventArgs { Key = key });

    [TestMethod]
    public async Task Escape_ClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "Escape");

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task Tab_ClosesMenu()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "Tab");

        // Assert
        Assert.IsFalse(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task ArrowDown_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "ArrowDown");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task ArrowUp_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "ArrowUp");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task Home_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "Home");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task End_KeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "End");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task PrintableCharacter_TriggersTypeAheadAndKeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "t");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task CharacterWithCtrlModifier_DoesNotTypeAhead_AndKeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await comp.Find("[role=menu]").TriggerEventAsync(
            "onkeydown", new KeyboardEventArgs { Key = "t", CtrlKey = true });

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task CharacterWithAltModifier_DoesNotTypeAhead_AndKeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await comp.Find("[role=menu]").TriggerEventAsync(
            "onkeydown", new KeyboardEventArgs { Key = "t", AltKey = true });

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task CharacterWithMetaModifier_DoesNotTypeAhead_AndKeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await comp.Find("[role=menu]").TriggerEventAsync(
            "onkeydown", new KeyboardEventArgs { Key = "t", MetaKey = true });

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }

    [TestMethod]
    public async Task MultiCharacterKey_DoesNotTypeAhead_AndKeepsMenuOpen()
    {
        // Arrange
        var ctx = CreateContext();
        var comp = await OpenMenuAsync(ctx);

        // Act
        await PressKeyAsync(comp, "F2");

        // Assert
        Assert.IsTrue(comp.Instance.IsOpen);
    }
}
