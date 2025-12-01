using D20Tek.BlazorComponents.Utilities;

namespace D20Tek.BlazorComponents.Core.UnitTests.Utilities;

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
    public void ThrowWhenEmpty_WithEmptyText()
    {
        // arrange
        string text = "";

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () => text.ThrowWhenEmpty("param"));
    }
}
