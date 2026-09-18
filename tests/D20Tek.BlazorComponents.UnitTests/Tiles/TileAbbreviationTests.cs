namespace D20Tek.BlazorComponents.UnitTests.Tiles;

[TestClass]
public class TileAbbreviationTests
{
    [TestMethod]
    public void Compute_WithOverride_ReturnsUppercasedOverride()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("Some Title", "desc", "xy");

        // Assert
        Assert.AreEqual("XY", result);
    }

    [TestMethod]
    public void Compute_WithLongOverride_TruncatesToTwoChars()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("Some Title", "desc", "abcd");

        // Assert
        Assert.AreEqual("AB", result);
    }

    [TestMethod]
    public void Compute_WithSingleCharOverride_ReturnsSingleUppercased()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute(null, null, "a");

        // Assert
        Assert.AreEqual("A", result);
    }

    [TestMethod]
    public void Compute_WithTwoWordTitle_ReturnsFirstLettersOfFirstTwoWords()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("Mountain Retreat", null, null);

        // Assert
        Assert.AreEqual("MR", result);
    }

    [TestMethod]
    public void Compute_WithThreeWordTitle_UsesOnlyFirstTwoWords()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("the quick brown", null, null);

        // Assert
        Assert.AreEqual("TQ", result);
    }

    [TestMethod]
    public void Compute_WithSingleWordTitle_ReturnsSingleInitial()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("Solo", null, null);

        // Assert
        Assert.AreEqual("S", result);
    }

    [TestMethod]
    public void Compute_WithEmptyTitleAndDescription_ReturnsFirstDescriptionChar()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("   ", "example", null);

        // Assert
        Assert.AreEqual("E", result);
    }

    [TestMethod]
    public void Compute_WithNoTitleDescriptionOrOverride_ReturnsQuestionMark()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute(null, null, null);

        // Assert
        Assert.AreEqual("?", result);
    }

    [TestMethod]
    public void Compute_WithWhitespaceOverride_FallsBackToTitle()
    {
        // Arrange

        // Act
        var result = TileAbbreviation.Compute("Beta Version", null, "   ");

        // Assert
        Assert.AreEqual("BV", result);
    }
}
