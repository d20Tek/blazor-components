namespace D20Tek.BlazorComponents.UnitTests.Timer;

[TestClass]
public class TimeDisplayFormatterTests
{
    [TestMethod]
    [DataRow(3, 15, 7, 18, "3D 15:07:18", DisplayName = "FormatWithDays")]
    [DataRow(0, 5, 27, 8, "5:27:08", DisplayName = "FormatWithoutDays")]
    [DataRow(0, 0, 27, 48, "0:27:48", DisplayName = "FormatWithoutHours")]
    [DataRow(0, 0, 0, 1, "0:00:01", DisplayName = "FormatWithOneSecond")]
    public void FormatTimeSpanRemaining_WithDays(int d, int hr, int min, int sec, string expected)
    {
        // arrange

        // act
        var result = TimeDisplayFormatter.FormatTimeSpanRemaining(new TimeSpan(d, hr, min, sec), "Done");

        // assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result);
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
    public void FormatTimeSpanRemaining_WithEmptyExpirationMessage()
    {
        // arrange

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage] () =>
            _ = TimeDisplayFormatter.FormatTimeSpanRemaining(TimeSpan.Zero, ""));
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
    public void FormatTimeRemaining_WithEmptyExpirationMessage()
    {
        // arrange

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException> ([ExcludeFromCodeCoverage] () =>
            _ = TimeDisplayFormatter.FormatTimeRemaining(42, ""));
    }

    [TestMethod]
    [DataRow(12, 5, 36, "12:05:36", DisplayName = "FormatWithHoursMinSec")]
    [DataRow(0, 15, 8, "15:08", DisplayName = "FormatWithMinSec")]
    [DataRow(0, 0, 19, "0:19", DisplayName = "FormatWithSec")]
    public void FormatTimeRemaining_WithHoursMinSec(int hr, int min, int sec, string expected)
    {
        // arrange

        // act
        var result = TimeDisplayFormatter.FormatTimeRemaining(hr, min, sec);

        // assert
        Assert.AreEqual(expected, result);
    }
}
