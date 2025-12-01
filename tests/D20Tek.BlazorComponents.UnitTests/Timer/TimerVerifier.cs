using AngleSharp.Diffing.Core;
using System.Linq;

namespace D20Tek.BlazorComponents.UnitTests.Timer;

internal static class TimerVerifier
{
    [ExcludeFromCodeCoverage]
    internal static void VerifyMarkupDifferences(IReadOnlyList<IDiff> results)
    {
        Assert.IsLessThanOrEqualTo(1, results.Count);
        if (results.Any())
        {
            var source = (AttrDiff)results[0];
            Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
        }
    }
}
