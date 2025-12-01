using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities;

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
        var color = "green";
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
        var color = "";
        var builder = new StyleBuilder("foo", "bar");

        // act
        builder.AddStyle("color", color, () => string.IsNullOrWhiteSpace(color) == false);

        // assert
        Assert.AreEqual("foo: bar;", builder.Build());
    }

    [TestMethod]
    public void AddClass_WithNullProperty()
    {
        // arrange
        var builder = new StyleBuilder("foo", "bar");

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            builder.AddStyle(null!, "value"));
    }

    [TestMethod]
    public void AddClass_WithNullValue()
    {
        // arrange
        var builder = new StyleBuilder("foo", "bar");

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            builder.AddStyle("property", null!));
    }
}
