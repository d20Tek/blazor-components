using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities;

[TestClass]
public class StyleBuilderFromAttributesTests
{
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
    public void AddStyleFromAttributes_WithNullAttributes()
    {
        // arrange
        var builder = new StyleBuilder();

        // act
        builder.AddStyleFromAttributes(null!);

        // assert
        Assert.IsNull(builder.Build());
    }
}
