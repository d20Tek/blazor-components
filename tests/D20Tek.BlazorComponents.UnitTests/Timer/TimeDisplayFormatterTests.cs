//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests.Timer
{
    [TestClass]
    public class TimeDisplayFormatterTests
    {
        [TestMethod]
        public void FormatTimeSpanRemaining_WithDays()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(3, 15, 7, 18), "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("3D 15:07:18", result);
        }

        [TestMethod]
        public void FormatTimeSpanRemaining_WithoutDays()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(5, 27, 8), "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("5:27:08", result);
        }

        [TestMethod]
        public void FormatTimeSpanRemaining_WithoutHours()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(0, 27, 48), "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("0:27:48", result);
        }

        [TestMethod]
        public void FormatTimeSpanRemaining_WithOneSecond()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(0, 0, 1), "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("0:00:01", result);
        }

        [TestMethod]
        public void FormatTimeSpanRemaining_Expired()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(TimeSpan.Zero, "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Done", result);
        }

        [TestMethod]
        public void FormatTimeSpanRemaining_WithNegativeTimeSpan()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(-2, 30, 30), "Done");

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Done", result);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [ExcludeFromCodeCoverage]
        public void FormatTimeSpanRemaining_WithEmptyExpirationMessage()
        {
            // arrange

            // act
            _ = TimeDisplayFormatter.FormatTimeSpanRemaining(TimeSpan.Zero, "");

            // assert
        }

        [TestMethod]
        public void FormatTimeRemaining_WithTimeValue()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeRemaining(8000, "Done");

            // assert
            Assert.AreEqual("2:13:20", result);
        }

        [TestMethod]
        public void FormatTimeRemaining_Expired()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeRemaining(-5, "Done");

            // assert
            Assert.AreEqual("Done", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [ExcludeFromCodeCoverage]
        public void FormatTimeRemaining_WithEmptyExpirationMessage()
        {
            // arrange

            // act
            _ = TimeDisplayFormatter.FormatTimeRemaining(42, "");

            // assert
        }

        [TestMethod]
        public void FormatTimeRemaining_WithHoursMinSec()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeRemaining(12, 5, 36);

            // assert
            Assert.AreEqual("12:05:36", result);
        }

        [TestMethod]
        public void FormatTimeRemaining_WithMinSec()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeRemaining(0, 15, 8);

            // assert
            Assert.AreEqual("15:08", result);
        }

        [TestMethod]
        public void FormatTimeRemaining_WithSeconds()
        {
            // arrange

            // act
            var result = TimeDisplayFormatter.FormatTimeRemaining(0, 0, 19);

            // assert
            Assert.AreEqual("0:19", result);
        }

    }
}
