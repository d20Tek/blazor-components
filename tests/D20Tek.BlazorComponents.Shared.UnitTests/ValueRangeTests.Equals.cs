namespace D20Tek.BlazorComponents.Core.UnitTests;

[TestClass]
public class ValueRangeEqualsTests
{
    [TestMethod]
    public void EqualityOperator_Equals()
    {
        // arrange
        var range1 = new ValueRange(5, 10);
        var range2 = new ValueRange(5, 10);

        // act
        var shouldBeEqual = range1 == range2;
        var shouldNotBeEqual = range1 != range2;

        // assert
        Assert.IsTrue(shouldBeEqual);
        Assert.IsFalse(shouldNotBeEqual);
    }

    [TestMethod]
    public void EqualityOperator_DoesNotEqual()
    {
        // arrange
        var range1 = new ValueRange(5, 10);
        var range2 = new ValueRange(8, 10);

        // act
        var shouldBeEqual = range1 == range2;
        var shouldNotBeEqual = range1 != range2;

        // assert
        Assert.IsFalse(shouldBeEqual);
        Assert.IsTrue(shouldNotBeEqual);
    }

    [TestMethod]
    public void Object_Equals()
    {
        // arrange
        object range1 = new ValueRange(5, 10);
        object range2 = new ValueRange(5, 10);

        // act
        var actual = range1.Equals(range2);

        // assert
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void Object_DoesNotEqual()
    {
        // arrange
        object range1 = new ValueRange(5, 10);
        object range2 = new ValueRange(6, 10);

        // act
        var actual = range1.Equals(range2);

        // assert
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public void Object_DoesNotEqual_OtherIsNull()
    {
        // arrange
        object range1 = new ValueRange(5, 10);

        // act
        var actual = range1.Equals(null);

        // assert
        Assert.IsFalse(actual);
    }

    [TestMethod]
    public void Object_Equals_WithNullMax()
    {
        // arrange
        object range1 = new ValueRange(5, null);
        object range2 = new ValueRange(5, null);

        // act
        var actual = range1.Equals(range2);

        // assert
        Assert.IsTrue(actual);
    }

    [TestMethod]
    public void Object_GetHashCode()
    {
        // arrange
        object range = new ValueRange(5, 10);
        var expected = 5 ^ 10;

        // act
        var actual = range.GetHashCode();

        // assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetHashcode_NullMax()
    {
        // arrange
        var range = new ValueRange(5, null);
        var expected = 5;

        // act
        var result = range.InRange(12);
        var actual = range.GetHashCode();

        // assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, actual);
    }
}
