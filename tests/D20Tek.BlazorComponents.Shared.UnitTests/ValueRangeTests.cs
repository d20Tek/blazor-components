namespace D20Tek.BlazorComponents.Core.UnitTests;

[TestClass]
public class ValueRangeTests
{
    [TestMethod]
    [DataRow(5, 10, DisplayName = "Create")]
    [DataRow(5, 5, DisplayName = "EqualMinMax")]
    [DataRow(5, null, DisplayName = "NullMax")]
    public void Create(int min, int? max)
    {
        // arrange

        // act
        var range = new ValueRange(min, max);

        // assert
        Assert.AreEqual(min, range.Min);
        Assert.AreEqual(max, range.Max);
    }

    [TestMethod]
    [DataRow(5, 10, 7, DisplayName = "Inclusive")]
    [DataRow(5, 10, 5, DisplayName = "InclusiveEqualsMin")]
    [DataRow(5, 10, 10, DisplayName = "InclusiveEqualsMax")]
    [DataRow(5, 5, 5, DisplayName = "InclusiveEqualsSingleValue")]
    [DataRow(2, null, 5, DisplayName = "InclusiveNullMax")]
    [DataRow(-5, 5, -2, DisplayName = "InclusiveNegativeValue")]
    public void InRange_Inclusive(int min, int? max, int value)
    {
        // arrange
        var range = new ValueRange(min, max);

        // act
        var actual = range.InRange(value);

        // assert
        Assert.IsTrue(actual);
    }

    [TestMethod]
    [DataRow(5, 10, 2, DisplayName = "LessThanRange")]
    [DataRow(5, 10, 13, DisplayName = "GreaterThanRange")]
    [DataRow(5, 5, 7, DisplayName = "NotEqualSingleNumber")]
    [DataRow(5, null, 1, DisplayName = "LessThanRangeWithNullMax")]
    public void InRange_ReturnsFalse(int min, int? max, int value)
    {
        // arrange
        var range = new ValueRange(min, max);

        // act
        var actual = range.InRange(value);

        // assert
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public void AssertInRange_NonInclusive()
    {
        // arrange
        var range = new ValueRange(5, 10);

        // act
        range.AssertInRange(7);

        // assert
    }

    [TestMethod]
    public void AssertInRange_NotInRange()
    {
        // arrange
        var range = new ValueRange(5, 10);

        // act - assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () => range.AssertInRange(0));
    }

    [TestMethod]
    public void Create_LargerMinimum()
    {
        // arrange

        // act - assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>  _ = new ValueRange(10, 9));
    }
}
