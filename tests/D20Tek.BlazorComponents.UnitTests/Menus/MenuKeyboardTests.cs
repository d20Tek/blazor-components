namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class MenuKeyboardTests
{
    private static MenuKeyCommand Resolve(string key, bool ctrl = false, bool alt = false, bool meta = false) =>
        MenuKeyboard.Resolve(new KeyboardEventArgs { Key = key, CtrlKey = ctrl, AltKey = alt, MetaKey = meta });

    [TestMethod]
    public void Resolve_Escape_ReturnsCloseAndReturnFocus()
    {
        // Arrange & Act
        var result = Resolve("Escape");

        // Assert
        Assert.AreEqual(MenuKeyCommand.CloseAndReturnFocus, result);
    }

    [TestMethod]
    public void Resolve_Tab_ReturnsClose()
    {
        // Arrange & Act
        var result = Resolve("Tab");

        // Assert
        Assert.AreEqual(MenuKeyCommand.Close, result);
    }

    [TestMethod]
    public void Resolve_ArrowDown_ReturnsFocusNext()
    {
        // Arrange & Act
        var result = Resolve("ArrowDown");

        // Assert
        Assert.AreEqual(MenuKeyCommand.FocusNext, result);
    }

    [TestMethod]
    public void Resolve_ArrowUp_ReturnsFocusPrevious()
    {
        // Arrange & Act
        var result = Resolve("ArrowUp");

        // Assert
        Assert.AreEqual(MenuKeyCommand.FocusPrevious, result);
    }

    [TestMethod]
    public void Resolve_Home_ReturnsFocusFirst()
    {
        // Arrange & Act
        var result = Resolve("Home");

        // Assert
        Assert.AreEqual(MenuKeyCommand.FocusFirst, result);
    }

    [TestMethod]
    public void Resolve_End_ReturnsFocusLast()
    {
        // Arrange & Act
        var result = Resolve("End");

        // Assert
        Assert.AreEqual(MenuKeyCommand.FocusLast, result);
    }

    [TestMethod]
    public void Resolve_ForSinglePrintableCharacter_ReturnsTypeAhead()
    {
        // Arrange & Act
        var result = Resolve("a");

        // Assert
        Assert.AreEqual(MenuKeyCommand.TypeAhead, result);
    }

    [TestMethod]
    public void Resolve_CharacterWithCtrl_ReturnsNone()
    {
        // Arrange & Act
        var result = Resolve("a", ctrl: true);

        // Assert
        Assert.AreEqual(MenuKeyCommand.None, result);
    }

    [TestMethod]
    public void Resolve_CharacterWithAlt_ReturnsNone()
    {
        // Arrange & Act
        var result = Resolve("a", alt: true);

        // Assert
        Assert.AreEqual(MenuKeyCommand.None, result);
    }

    [TestMethod]
    public void Resolve_CharacterWithMeta_ReturnsNone()
    {
        // Arrange & Act
        var result = Resolve("a", meta: true);

        // Assert
        Assert.AreEqual(MenuKeyCommand.None, result);
    }

    [TestMethod]
    public void Resolve_ForMultiCharacterKey_ReturnsNone()
    {
        // Arrange & Act
        var result = Resolve("F2");

        // Assert
        Assert.AreEqual(MenuKeyCommand.None, result);
    }

    [TestMethod]
    public void Resolve_ForUnhandledNamedKey_ReturnsNone()
    {
        // Arrange & Act
        var result = Resolve("Enter");

        // Assert
        Assert.AreEqual(MenuKeyCommand.None, result);
    }
}
