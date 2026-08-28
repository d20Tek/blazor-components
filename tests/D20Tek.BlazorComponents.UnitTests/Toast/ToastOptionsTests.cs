namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastOptionsTests
{
    [TestMethod]
    public void Defaults_AreExpected()
    {
        // arrange - act
        var options = new ToastOptions();

        // assert
        Assert.AreEqual(NotificationVariant.Info, options.Variant);
        Assert.AreEqual(ToastPosition.BottomRight, options.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(5), options.Timeout);
        Assert.IsTrue(options.ShowIcon);
        Assert.IsTrue(options.Dismissible);
        Assert.IsTrue(options.Animate);
        Assert.IsFalse(options.IsSticky);
    }

    [TestMethod]
    public void IsSticky_TrueWhenTimeoutZero()
    {
        // arrange - act
        var options = new ToastOptions { Timeout = TimeSpan.Zero };

        // assert
        Assert.IsTrue(options.IsSticky);
    }

    [TestMethod]
    public void IsSticky_TrueWhenTimeoutNegative()
    {
        // arrange - act
        var options = new ToastOptions { Timeout = TimeSpan.FromSeconds(-1) };

        // assert
        Assert.IsTrue(options.IsSticky);
    }
}
