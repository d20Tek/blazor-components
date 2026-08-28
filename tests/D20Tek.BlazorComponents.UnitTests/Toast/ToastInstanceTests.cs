namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastInstanceTests
{
    private static readonly RenderFragment _content = [ExcludeFromCodeCoverage](builder) => builder.AddContent(0, "x");

    [TestMethod]
    public void Defaults_AreExpected()
    {
        // arrange - act
        var toast = new ToastInstance { Content = _content };

        // assert
        Assert.AreNotEqual(Guid.Empty, toast.Id);
        Assert.AreEqual(NotificationVariant.Info, toast.Variant);
        Assert.AreEqual(ToastPosition.BottomRight, toast.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(5), toast.Timeout);
        Assert.IsTrue(toast.ShowIcon);
        Assert.IsTrue(toast.Dismissible);
        Assert.IsTrue(toast.Animate);
        Assert.IsFalse(toast.IsSticky);
        Assert.AreNotEqual(default, toast.CreatedAt);
    }

    [TestMethod]
    public void Ids_AreUniquePerInstance()
    {
        // arrange - act
        var a = new ToastInstance { Content = _content };
        var b = new ToastInstance { Content = _content };

        // assert
        Assert.AreNotEqual(a.Id, b.Id);
    }

    [TestMethod]
    public void IsSticky_TrueWhenTimeoutZero()
    {
        // arrange - act
        var toast = new ToastInstance { Content = _content, Timeout = TimeSpan.Zero };

        // assert
        Assert.IsTrue(toast.IsSticky);
    }
}
