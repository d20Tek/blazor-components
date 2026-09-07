namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastDependencyInjectionTests
{
    [TestMethod]
    public void AddToast_RegistersToastServiceAsScoped()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddToast();

        // assert
        var descriptor = services.Single(d => d.ServiceType == typeof(IToastService));
        Assert.AreEqual(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    [TestMethod]
    public void AddToast_ResolvesToastService()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddToast();
        var provider = services.BuildServiceProvider();

        // act
        var service = provider.GetService<IToastService>();

        // assert
        Assert.IsNotNull(service);
    }

    [TestMethod]
    public void AddToast_ReturnsSameServiceCollection()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        var result = services.AddToast();

        // assert
        Assert.AreSame(services, result);
    }

    [TestMethod]
    public void AddToast_WithoutConfigure_RegistersDefaultToastDefaults()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddToast();
        var provider = services.BuildServiceProvider();

        // act
        var defaults = provider.GetService<ToastDefaults>();

        // assert
        Assert.IsNotNull(defaults);
        Assert.AreEqual(ToastPosition.BottomCenter, defaults.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(3), defaults.DefaultTimeout);
    }

    [TestMethod]
    public void AddToast_RegistersToastDefaultsAsSingleton()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddToast(o => o.Position = ToastPosition.TopLeft);

        // assert
        var descriptor = services.Single(d => d.ServiceType == typeof(ToastDefaults));
        Assert.AreEqual(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [TestMethod]
    public void AddToast_WithConfigure_AppliesConfiguredDefaults()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddToast(o =>
        {
            o.Position = ToastPosition.TopCenter;
            o.DefaultTimeout = TimeSpan.FromSeconds(10);
            o.ShowIcon = false;
            o.Dismissible = false;
            o.Animate = false;
        });
        var provider = services.BuildServiceProvider();

        // act
        var defaults = provider.GetRequiredService<ToastDefaults>();

        // assert
        Assert.AreEqual(ToastPosition.TopCenter, defaults.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(10), defaults.DefaultTimeout);
        Assert.IsFalse(defaults.ShowIcon);
        Assert.IsFalse(defaults.Dismissible);
        Assert.IsFalse(defaults.Animate);
    }

    [TestMethod]
    public void AddToast_WithConfigure_ServiceExposesConfiguredDefaults()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddToast(o => o.Position = ToastPosition.TopRight);
        var provider = services.BuildServiceProvider();

        // act
        var service = provider.GetRequiredService<IToastService>();

        // assert
        Assert.AreEqual(ToastPosition.TopRight, service.Defaults.Position);
    }
}
