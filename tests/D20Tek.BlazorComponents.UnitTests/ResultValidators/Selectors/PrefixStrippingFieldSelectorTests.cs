using D20Tek.BlazorComponents;
using D20Tek.BlazorComponents.Selectors;
using Microsoft.AspNetCore.Components.Forms;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class PrefixStrippingFieldSelectorTests
{
    private static FieldSelectorContext CreateContext()
    {
        var model = new object();
        return new FieldSelectorContext(new EditContext(model), model.GetType());
    }

    private sealed class StaticSelector(params string[] names) : IErrorFieldSelector
    {
        public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context) => names;
    }

    [TestMethod]
    public void GetFieldNames_StripsFirstMatchingPrefix()
    {
        // arrange
        var inner = new StaticSelector("User.Name");
        var sut = new PrefixStrippingFieldSelector(inner, ["User.", "Account."]);

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_UnmatchedPrefix_ReturnsInnerValueUnchanged()
    {
        // arrange
        var inner = new StaticSelector("Order.Total");
        var sut = new PrefixStrippingFieldSelector(inner, ["User."]);

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Order.Total"], result);
    }

    [TestMethod]
    public void GetFieldNames_EmptyValue_PassesThroughUnchanged()
    {
        // arrange
        var inner = new StaticSelector(string.Empty);
        var sut = new PrefixStrippingFieldSelector(inner, ["User."]);

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual([string.Empty], result);
    }

    [TestMethod]
    public void GetFieldNames_CaseInsensitiveByDefault()
    {
        // arrange
        var inner = new StaticSelector("user.Name");
        var sut = new PrefixStrippingFieldSelector(inner, ["User."]);

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_OrdinalComparison_RespectsCase()
    {
        // arrange
        var inner = new StaticSelector("user.Name");
        var sut = new PrefixStrippingFieldSelector(inner, ["User."], StringComparison.Ordinal);

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["user.Name"], result);
    }

    [TestMethod]
    public void Ctor_NullInner_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new PrefixStrippingFieldSelector(null!, ["User."]));
    }

    [TestMethod]
    public void Ctor_NullPrefixes_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new PrefixStrippingFieldSelector(new StaticSelector(), null!));
    }
}
