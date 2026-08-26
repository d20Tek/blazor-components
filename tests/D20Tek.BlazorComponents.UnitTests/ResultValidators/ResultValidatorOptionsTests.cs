using D20Tek.BlazorComponents.Selectors;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators;

[TestClass]
public sealed class ResultValidatorOptionsTests
{
    [TestMethod]
    public void Defaults_UseCodeAsFieldAndPassThrough()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act
        var selector = sut.FieldSelector;
        var behavior = sut.UnknownFieldBehavior;

        // assert
        Assert.AreSame(CodeAsFieldSelector.Instance, selector);
        Assert.AreEqual(UnknownFieldBehavior.PassThrough, behavior);
    }

    [TestMethod]
    public void UseSelector_ReplacesFieldSelector()
    {
        // arrange
        var sut = new ResultValidatorOptions();
        var custom = new DelegateFieldSelector([ExcludeFromCodeCoverage](_) => "x");

        // act
        var returned = sut.UseSelector(custom);

        // assert
        Assert.AreSame(sut, returned);
        Assert.AreSame(custom, sut.FieldSelector);
    }

    [TestMethod]
    public void UseSelector_NullSelector_Throws()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() => sut.UseSelector(null!));
    }

    [TestMethod]
    public void UseCodeAsField_ResetsToCodeAsFieldSelector()
    {
        // arrange
        var sut = new ResultValidatorOptions();
        sut.UseSelector(new DelegateFieldSelector([ExcludeFromCodeCoverage](_) => "x"));

        // act
        sut.UseCodeAsField();

        // assert
        Assert.AreSame(CodeAsFieldSelector.Instance, sut.FieldSelector);
    }

    [TestMethod]
    public void UseDisplayNames_SetsDisplayNameSelector()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act
        sut.UseDisplayNames();

        // assert
        Assert.IsInstanceOfType<DisplayNameFieldSelector>(sut.FieldSelector);
    }

    [TestMethod]
    public void UseJsonPropertyNames_SetsJsonPropertyNameSelector()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act
        sut.UseJsonPropertyNames();

        // assert
        Assert.IsInstanceOfType<JsonPropertyNameFieldSelector>(sut.FieldSelector);
    }

    [TestMethod]
    public void StripPrefixes_WrapsCurrentSelector()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act
        sut.StripPrefixes("User.");

        // assert
        Assert.IsInstanceOfType<PrefixStrippingFieldSelector>(sut.FieldSelector);
    }

    [TestMethod]
    public void Compose_WrapsCurrentAndAdditionalSelectors()
    {
        // arrange
        var sut = new ResultValidatorOptions();
        var extra = new DelegateFieldSelector([ExcludeFromCodeCoverage](_) => "y");

        // act
        sut.Compose(extra);

        // assert
        Assert.IsInstanceOfType<CompositeFieldSelector>(sut.FieldSelector);
    }

    [TestMethod]
    public void FluentApi_IsChainable()
    {
        // arrange
        var sut = new ResultValidatorOptions();

        // act
        sut.UseJsonPropertyNames().StripPrefixes("User.");

        // assert
        Assert.IsInstanceOfType<PrefixStrippingFieldSelector>(sut.FieldSelector);
    }

    [TestMethod]
    public void UnknownFieldBehavior_IsSettable()
    {
        // arrange
        var sut = new ResultValidatorOptions
        {
            // act
            UnknownFieldBehavior = UnknownFieldBehavior.Throw
        };

        // assert
        Assert.AreEqual(UnknownFieldBehavior.Throw, sut.UnknownFieldBehavior);
    }
}
