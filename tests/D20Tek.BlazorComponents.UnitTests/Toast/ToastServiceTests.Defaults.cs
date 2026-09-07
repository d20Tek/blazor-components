namespace D20Tek.BlazorComponents.UnitTests.Toast;

public sealed partial class ToastServiceTests
{
    private static readonly RenderFragment _defaultsContent =
        [ExcludeFromCodeCoverage](builder) => builder.AddContent(0, "hi");

    [TestMethod]
    public void Show_WithAppDefaults_SeedsToastFromDefaults()
    {
        // arrange
        var defaults = new ToastDefaults
        {
            Position = ToastPosition.TopLeft,
            DefaultTimeout = TimeSpan.FromSeconds(12),
            ShowIcon = false,
            Dismissible = false,
            Animate = false,
        };
        var service = new ToastService(defaults);
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show(_defaultsContent);

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(ToastPosition.TopLeft, captured.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(12), captured.Timeout);
        Assert.IsFalse(captured.ShowIcon);
        Assert.IsFalse(captured.Dismissible);
        Assert.IsFalse(captured.Animate);
    }

    [TestMethod]
    public void Show_WithConfigure_OverridesAppDefaults()
    {
        // arrange
        var defaults = new ToastDefaults
        {
            Position = ToastPosition.TopLeft,
            DefaultTimeout = TimeSpan.FromSeconds(12),
            ShowIcon = false,
        };
        var service = new ToastService(defaults);
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show(_defaultsContent, o =>
        {
            o.Position = ToastPosition.BottomRight;
            o.Timeout = TimeSpan.FromSeconds(3);
            o.ShowIcon = true;
        });

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(ToastPosition.BottomRight, captured.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(3), captured.Timeout);
        Assert.IsTrue(captured.ShowIcon);
    }

    [TestMethod]
    public void Defaults_WhenConstructedWithoutDefaults_ReturnsBuiltInDefaults()
    {
        // arrange
        var service = new ToastService();

        // act
        var defaults = service.Defaults;

        // assert
        Assert.IsNotNull(defaults);
        Assert.AreEqual(ToastPosition.BottomCenter, defaults.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(3), defaults.DefaultTimeout);
        Assert.IsTrue(defaults.ShowIcon);
        Assert.IsTrue(defaults.Dismissible);
        Assert.IsTrue(defaults.Animate);
    }
}
