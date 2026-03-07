using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace D20Tek.BlazorComponents.UnitTests.Markdown;

[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public void AddMarkdownRenderer_RegistersIMarkdownRendererAsSingleton()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddMarkdownRenderer();

        // assert
        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMarkdownRenderer));
        Assert.IsNotNull(descriptor);
        Assert.AreEqual(ServiceLifetime.Singleton, descriptor.Lifetime);
        Assert.AreEqual(typeof(MarkdigRenderer), descriptor.ImplementationType);
    }

    [TestMethod]
    public void AddMarkdownRenderer_ReturnsServiceCollection()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        var result = services.AddMarkdownRenderer();

        // assert
        Assert.AreSame(services, result);
    }
}
