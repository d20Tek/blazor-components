//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using D20Tek.BlazorComponents.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void NullIfEmpty_WithText()
        {
            // arrange
            string text = "test";

            // act
            var result = text.NullIfEmpty();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(text, result);
        }

        [TestMethod]
        public void NullIfEmpty_WithEmptyText()
        {
            // arrange
            string text = "";

            // act
            var result = text.NullIfEmpty();

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ThrowWhenEmpty_WithText()
        {
            // arrange
            string text = "test";

            // act
            text.ThrowWhenEmpty("param");

            // assert
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowWhenEmpty_WithEmptyText()
        {
            // arrange
            string text = "";

            // act
            text.ThrowWhenEmpty("param");

            // assert
        }
    }
}
