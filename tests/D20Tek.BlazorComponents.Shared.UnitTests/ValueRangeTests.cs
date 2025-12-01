using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.BlazorComponents.Core.UnitTests
{
    [TestClass]
    public class ValueRangeTests
    {
        [TestMethod]
        public void Create()
        {
            // arrange

            // act
            var range = new ValueRange(5, 10);

            // assert
            Assert.AreEqual(5, range.Min);
            Assert.AreEqual(10, range.Max);
        }

        [TestMethod]
        public void Create_EqualMinMax()
        {
            // arrange

            // act
            var range = new ValueRange(5, 5);

            // assert
            Assert.AreEqual(5, range.Min);
            Assert.AreEqual(5, range.Max);
        }

        [TestMethod]
        public void Create_WithNullMax()
        {
            // arrange

            // act
            var range = new ValueRange(5, null);

            // assert
            Assert.AreEqual(5, range.Min);
            Assert.IsNull(range.Max);
        }

        [TestMethod]
        public void InRange_NonInclusive()
        {
            // arrange
            var range = new ValueRange(5, 10);

            // act
            var actual = range.InRange(7);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_InclusiveMin()
        {
            // arrange
            var range = new ValueRange(5, 10);

            // act
            var actual = range.InRange(5);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_InclusiveMax()
        {
            // arrange
            var range = new ValueRange(5, 10);

            // act
            var actual = range.InRange(10);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_SingleNumber()
        {
            // arrange
            var range = new ValueRange(5, 5);

            // act
            var actual = range.InRange(5);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_NullMax()
        {
            // arrange
            var range = new ValueRange(2, null);

            // act
            var actual = range.InRange(5);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_NegativeValue()
        {
            // arrange
            var range = new ValueRange(-5, 5);

            // act
            var actual = range.InRange(-2);

            // assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void InRange_LessThanRange()
        {
            // arrange
            var range = new ValueRange(5, 10);

            // act
            var actual = range.InRange(3);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void InRange_GreaterThanRange()
        {
            // arrange
            var range = new ValueRange(5, 10);

            // act
            var actual = range.InRange(13);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void InRange_NotEqualSingle()
        {
            // arrange
            var range = new ValueRange(5, 5);

            // act
            var actual = range.InRange(7);

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
        public void Object_OtherIsNull()
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
        public void Create_LargerMinimum()
        {
            // arrange

            // act - assert
            Assert.ThrowsExactly<ArgumentOutOfRangeException>([ExcludeFromCodeCoverage] () =>  _ = new ValueRange(10, 9));
        }

        [TestMethod]
        public void Create_NullMax()
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
}
