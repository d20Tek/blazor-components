using D20Tek.BlazorComponents.UnitTests.Menus.Fakes;

namespace D20Tek.BlazorComponents.UnitTests.Menus;

[TestClass]
public class MenuCoordinatorTests
{
    [TestMethod]
    public async Task NotifyOpenedAsync_WithSingleMenu_DoesNotRequestClose()
    {
        // Arrange
        var group = Guid.NewGuid().ToString();
        var menu = new FakeManagedMenu();

        // Act
        await MenuCoordinator.NotifyOpenedAsync(group, menu);

        // Assert
        Assert.AreEqual(0, menu.CloseRequestCount);

        // Cleanup
        MenuCoordinator.NotifyClosed(group, menu);
    }

    [TestMethod]
    public async Task NotifyOpenedAsync_WithSecondMenuSameGroup_ClosesFirst()
    {
        // Arrange
        var group = Guid.NewGuid().ToString();
        var first = new FakeManagedMenu();
        var second = new FakeManagedMenu();

        // Act
        await MenuCoordinator.NotifyOpenedAsync(group, first);
        await MenuCoordinator.NotifyOpenedAsync(group, second);

        // Assert
        Assert.AreEqual(1, first.CloseRequestCount);
        Assert.AreEqual(0, second.CloseRequestCount);

        // Cleanup
        MenuCoordinator.NotifyClosed(group, second);
    }

    [TestMethod]
    public async Task NotifyOpenedAsync_WithDifferentGroups_DoesNotClose()
    {
        // Arrange
        var groupA = Guid.NewGuid().ToString();
        var groupB = Guid.NewGuid().ToString();
        var first = new FakeManagedMenu();
        var second = new FakeManagedMenu();

        // Act
        await MenuCoordinator.NotifyOpenedAsync(groupA, first);
        await MenuCoordinator.NotifyOpenedAsync(groupB, second);

        // Assert
        Assert.AreEqual(0, first.CloseRequestCount);
        Assert.AreEqual(0, second.CloseRequestCount);

        // Cleanup
        MenuCoordinator.NotifyClosed(groupA, first);
        MenuCoordinator.NotifyClosed(groupB, second);
    }

    [TestMethod]
    public async Task NotifyOpenedAsync_ReopeningSameMenu_DoesNotRequestClose()
    {
        // Arrange
        var group = Guid.NewGuid().ToString();
        var menu = new FakeManagedMenu();

        // Act
        await MenuCoordinator.NotifyOpenedAsync(group, menu);
        await MenuCoordinator.NotifyOpenedAsync(group, menu);

        // Assert
        Assert.AreEqual(0, menu.CloseRequestCount);

        // Cleanup
        MenuCoordinator.NotifyClosed(group, menu);
    }

    [TestMethod]
    public async Task NotifyClosed_WithDifferentMenu_LeavesTrackedMenuInPlace()
    {
        // Arrange
        var group = Guid.NewGuid().ToString();
        var tracked = new FakeManagedMenu();
        var other = new FakeManagedMenu();
        await MenuCoordinator.NotifyOpenedAsync(group, tracked);

        // Act
        MenuCoordinator.NotifyClosed(group, other);
        var next = new FakeManagedMenu();
        await MenuCoordinator.NotifyOpenedAsync(group, next);

        // Assert
        Assert.AreEqual(1, tracked.CloseRequestCount);

        // Cleanup
        MenuCoordinator.NotifyClosed(group, next);
    }
}
