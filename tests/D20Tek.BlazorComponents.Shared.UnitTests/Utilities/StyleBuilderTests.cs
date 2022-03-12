//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using D20Tek.BlazorComponents.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace D20Tek.BlazorComponents.Shared.UnitTests.Utilities
{
    [TestClass]
    public class StyleBuilderTests
    {
        [TestMethod]
        public void Constructor_WithValue()
        {
            // arrange

            // act
            var builder = new StyleBuilder("foo", "bar");

            // assert
            Assert.IsNotNull(builder);
            Assert.AreEqual("foo: bar;", builder.ToString());
        }

        [TestMethod]
        public void Constructor_Default()
        {
            // arrange

            // act
            var builder = new StyleBuilder();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.ToString());
        }

        [TestMethod]
        public void AddStyle_WithMultipleValues()
        {
            // arrange
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyle("color", "green");
            builder.AddStyle("border", "1px solid black");

            // assert
            Assert.AreEqual("foo: bar; color: green; border: 1px solid black;", builder.Build());
        }

        [TestMethod]
        public void AddStyle_WithTrueCondition()
        {
            // arrange
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyle("color", "green", true);

            // assert
            Assert.AreEqual("foo: bar; color: green;", builder.Build());
        }

        [TestMethod]
        public void AddStyle_WithFalseCondition()
        {
            // arrange
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyle("color", "green", false);

            // assert
            Assert.AreEqual("foo: bar;", builder.Build());
        }

        [TestMethod]
        public void AddStyle_WithTrueFunc()
        {
            // arrange
            string color = "green";
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyle("color", color, () => string.IsNullOrWhiteSpace(color) == false);

            // assert
            Assert.AreEqual("foo: bar; color: green;", builder.Build());
        }

        [TestMethod]
        public void AddStyle_WithFalseFunc()
        {
            // arrange
            string color = "";
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyle("color", color, () => string.IsNullOrWhiteSpace(color) == false);

            // assert
            Assert.AreEqual("foo: bar;", builder.Build());
        }

        [TestMethod]
        public void AddStyleFromAttributes_WithStyleValue()
        {
            // arrange
            var attributes = new Dictionary<string, object>
            {
                { "id", "failed" },
                { "class", "test-2 test-3" },
                { "style", "width: 20" }
            };
            var builder = new StyleBuilder();

            // act
            builder.AddStyleFromAttributes(attributes);

            // assert
            Assert.AreEqual("width: 20;", builder.Build());
        }

        [TestMethod]
        public void AddStyleFromAttributes_WithoutStyleValue()
        {
            // arrange
            var attributes = new Dictionary<string, object>
            {
                { "id", "failed" },
                { "class", "test-2 test-3" },
            };
            var builder = new StyleBuilder("foo", "bar");

            // act
            builder.AddStyleFromAttributes(attributes);

            // assert
            Assert.AreEqual("foo: bar;", builder.Build());
        }

        [TestMethod]
        public void AddClassFromAttributes_WithNullAttributes()
        {
            // arrange
            var builder = new StyleBuilder();

            // act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            builder.AddStyleFromAttributes(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // assert
            Assert.IsNull(builder.Build());
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddClass_WithNullProperty()
        {
            // arrange
            var builder = new StyleBuilder("foo", "bar");

            // act - assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            builder.AddStyle(null, "value");
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddClass_WithNullValue()
        {
            // arrange
            var builder = new StyleBuilder("foo", "bar");

            // act - assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            builder.AddStyle("property", null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }
}
