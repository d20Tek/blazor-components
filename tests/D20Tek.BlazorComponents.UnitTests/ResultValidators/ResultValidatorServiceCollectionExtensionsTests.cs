using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public sealed class ResultValidatorServiceCollectionExtensionsTests
{
    [TestMethod]
    public void AddResultValidator_WithoutConfigure_RegistersDefaults()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddResultValidator();
        var provider = services.BuildServiceProvider();

        // assert
        var options = provider.GetRequiredService<ResultValidatorOptions>();
        var selector = provider.GetRequiredService<IErrorFieldSelector>();

        Assert.AreSame(CodeAsFieldSelector.Instance, options.FieldSelector);
        Assert.AreSame(CodeAsFieldSelector.Instance, selector);
    }

    [TestMethod]
    public void AddResultValidator_WithConfigure_AppliesConfiguration()
    {
        // arrange
        var services = new ServiceCollection();

        // act
        services.AddResultValidator(o => o
            .UseJsonPropertyNames()
            .StripPrefixes("User."));
        var provider = services.BuildServiceProvider();

        // assert
        var selector = provider.GetRequiredService<IErrorFieldSelector>();
        Assert.IsInstanceOfType<PrefixStrippingFieldSelector>(selector);
    }

    [TestMethod]
    public void AddResultValidator_NullServices_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            ((IServiceCollection)null!).AddResultValidator());
    }

    [TestMethod]
    public void AddResultValidator_RegistersOptionsAsSingleton()
    {
        // arrange
        var services = new ServiceCollection();
        services.AddResultValidator();
        var provider = services.BuildServiceProvider();

        // act
        var first = provider.GetRequiredService<ResultValidatorOptions>();
        var second = provider.GetRequiredService<ResultValidatorOptions>();

        // assert
        Assert.AreSame(first, second);
    }
}
