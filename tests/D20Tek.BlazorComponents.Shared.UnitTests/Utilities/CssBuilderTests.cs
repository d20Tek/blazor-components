//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using D20Tek.BlazorComponents.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace D20Tek.BlazorComponents.Shared.UnitTests.Utilities
{
    [TestClass]
    public class CssBuilderTests
    {
        [TestMethod]
        public void Constructor_WithValue()
        {
            // arrange
            var className = "my-test-class";

            // act
            var builder = new CssBuilder(className);

            // assert
            Assert.IsNotNull(builder);
            Assert.AreEqual(className, builder.ToString());
        }

        [TestMethod]
        public void Constructor_Default()
        {
            // arrange

            // act
            var builder = new CssBuilder();

            // assert
            Assert.IsNotNull(builder);
            Assert.IsNull(builder.ToString());
        }

        [TestMethod]
        public void AddClass_WithMultipleValues()
        {
            // arrange
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClass("test-2");
            builder.AddClass("test-3");
            builder.AddClass("test-4");

            // assert
            Assert.AreEqual("my-test-class test-2 test-3 test-4", builder.Build());
        }

        [TestMethod]
        public void AddClass_WithTrueCondition()
        {
            // arrange
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClass("test-condition", true);

            // assert
            Assert.AreEqual("my-test-class test-condition", builder.Build());
        }

        [TestMethod]
        public void AddClass_WithFalseCondition()
        {
            // arrange
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClass("test-condition", false);

            // assert
            Assert.AreEqual("my-test-class", builder.Build());
        }

        [TestMethod]
        public void AddClass_WithTrueFunc()
        {
            // arrange
            var text = "test-condition";
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClass(text, () => string.IsNullOrEmpty(text) == false);

            // assert
            Assert.AreEqual("my-test-class test-condition", builder.Build());
        }

        [TestMethod]
        public void AddClass_WithFalseFunc()
        {
            // arrange
            var text = "";
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClass("failed", () => string.IsNullOrEmpty(text) == false);

            // assert
            Assert.AreEqual("my-test-class", builder.Build());
        }

        [TestMethod]
        public void AddClassFromAttributes_WithClassValue()
        {
            // arrange
            var attributes = new Dictionary<string, object>
            {
                { "id", "failed" },
                { "class", "test-2 test-3" },
                { "style", "width: 20;" }
            };
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClassFromAttributes(attributes);

            // assert
            Assert.AreEqual("my-test-class test-2 test-3", builder.Build());
        }


        [TestMethod]
        public void AddClassFromAttributes_WithoutClassValue()
        {
            // arrange
            var attributes = new Dictionary<string, object>
            {
                { "id", "failed" },
                { "style", "width: 20;" }
            };
            var builder = new CssBuilder("my-test-class");

            // act
            builder.AddClassFromAttributes(attributes);

            // assert
            Assert.AreEqual("my-test-class", builder.Build());
        }

        [TestMethod]
        public void AddClassFromAttributes_WithNullAttributes()
        {
            // arrange
            var builder = new CssBuilder();

            // act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            builder.AddClassFromAttributes(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // assert
            Assert.IsNull(builder.Build());
        }
    }
}