using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities;

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
}