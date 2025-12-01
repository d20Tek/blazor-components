using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities;

[TestClass]
public class CssBuilderFromAttributesTests
{
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
        builder.AddClassFromAttributes(null!);

        // assert
        Assert.IsNull(builder.Build());
    }
}
