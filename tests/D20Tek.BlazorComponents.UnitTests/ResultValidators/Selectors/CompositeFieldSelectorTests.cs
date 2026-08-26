using D20Tek.BlazorComponents.Selectors;
using Microsoft.AspNetCore.Components.Forms;

namespace D20Tek.BlazorComponents.UnitTests.ResultValidators.Selectors;

[TestClass]
public sealed class CompositeFieldSelectorTests
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
    public void GetFieldNames_FirstProducingSelectorWins()
    {
        // arrange
        var sut = new CompositeFieldSelector(
            new StaticSelector(string.Empty),
            new StaticSelector("Name"),
            new StaticSelector("Ignored"));

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["Name"], result);
    }

    [TestMethod]
    public void GetFieldNames_AllSelectorsEmpty_ReturnsFormLevelField()
    {
        // arrange
        var sut = new CompositeFieldSelector(
            new StaticSelector(string.Empty),
            new StaticSelector());

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual([IErrorFieldSelector.FormLevelField], result);
    }

    [TestMethod]
    public void GetFieldNames_MultipleValuesFromWinner_AllReturned()
    {
        // arrange
        var sut = new CompositeFieldSelector(
            new StaticSelector("A", "B"));

        // act
        var result = sut.GetFieldNames(Error.Validation("x", "y"), CreateContext()).ToArray();

        // assert
        Assert.AreSequenceEqual(["A", "B"], result);
    }

    [TestMethod]
    public void Ctor_NullArray_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() =>
            new CompositeFieldSelector(null!));
    }

    [TestMethod]
    public void Ctor_EmptyArray_Throws()
    {
        // act-assert
        Assert.ThrowsExactly<ArgumentException>([ExcludeFromCodeCoverage]() =>
            new CompositeFieldSelector([]));
    }
}
