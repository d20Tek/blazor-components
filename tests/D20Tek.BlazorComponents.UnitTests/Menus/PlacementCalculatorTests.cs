namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class PlacementCalculatorTests
{
    private static readonly BlazorComponents.MenuRect Viewport = new(0, 0, 1000, 800);

    [TestMethod]
    public void Calculate_BottomStart_WithAmpleSpace_KeepsSideAndAlignsLeft()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(100, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.BottomStart, result.ResolvedPlacement);
        Assert.AreEqual(100, result.X);
        Assert.AreEqual(126, result.Y);
    }

    [TestMethod]
    public void Calculate_BottomEnd_AlignsMenuRightEdgeToTriggerRightEdge()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(500, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomEnd, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.BottomEnd, result.ResolvedPlacement);
        Assert.AreEqual(340, result.X);
        Assert.AreEqual(126, result.Y);
    }

    [TestMethod]
    public void Calculate_BottomStart_WhenNoRoomBelow_FlipsToTop()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(100, 700, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.TopStart, result.ResolvedPlacement);
        Assert.AreEqual(544, result.Y);
    }

    [TestMethod]
    public void Calculate_TopStart_WhenNoRoomAbove_FlipsToBottom()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(100, 20, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.TopStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.BottomStart, result.ResolvedPlacement);
        Assert.AreEqual(46, result.Y);
    }

    [TestMethod]
    public void Calculate_WhenNeitherSideFits_KeepsPreferredSide()
    {
        // Arrange
        var tightBoundary = new BlazorComponents.MenuRect(0, 0, 1000, 160);
        var trigger = new BlazorComponents.MenuRect(100, 70, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, tightBoundary, BlazorComponents.MenuPlacement.BottomStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.BottomStart, result.ResolvedPlacement);
    }

    [TestMethod]
    public void Calculate_LeftStart_WhenNoRoomLeft_FlipsToRight()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(20, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.LeftStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.RightStart, result.ResolvedPlacement);
        Assert.AreEqual(66, result.X);
    }

    [TestMethod]
    public void Calculate_RightStart_WhenNoRoomRight_FlipsToLeft()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(940, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.RightStart, 6);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.LeftStart, result.ResolvedPlacement);
        Assert.AreEqual(734, result.X);
    }

    [TestMethod]
    public void Calculate_BottomPlacement_ShiftsMenuInsideLeftBoundary()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(0, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomEnd, 6);

        // Assert
        Assert.AreEqual(0, result.X);
    }

    [TestMethod]
    public void Calculate_BottomPlacement_ShiftsMenuInsideRightBoundary()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(960, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomStart, 6);

        // Assert
        Assert.AreEqual(800, result.X);
    }

    [TestMethod]
    public void Calculate_LeftPlacement_ShiftsMenuInsideVerticalBoundary()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(500, 780, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.LeftStart, 6);

        // Assert
        Assert.AreEqual(650, result.Y);
    }

    [TestMethod]
    public void Calculate_WithFlipDisabled_KeepsPreferredSideEvenWhenClipped()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(100, 700, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomStart, 6, flipEnabled: false);

        // Assert
        Assert.AreEqual(BlazorComponents.MenuPlacement.BottomStart, result.ResolvedPlacement);
        Assert.AreEqual(726, result.Y);
    }

    [TestMethod]
    public void Calculate_WithShiftDisabled_DoesNotClampToBoundary()
    {
        // Arrange
        var trigger = new BlazorComponents.MenuRect(960, 100, 40, 20);
        var menu = new BlazorComponents.MenuRect(0, 0, 200, 150);

        // Act
        var result = BlazorComponents.PlacementCalculator.Calculate(
            trigger, menu, Viewport, BlazorComponents.MenuPlacement.BottomStart, 6, shiftEnabled: false);

        // Assert
        Assert.AreEqual(960, result.X);
    }
}
