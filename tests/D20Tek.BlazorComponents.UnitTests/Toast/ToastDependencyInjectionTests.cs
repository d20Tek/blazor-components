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
}
