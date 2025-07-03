//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using AngleSharp.Diffing.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace D20Tek.BlazorComponents.UnitTests.Timer
{
    internal static class TimerVerifier
    {
        [ExcludeFromCodeCoverage]
        internal static void VerifyMarkupDifferences(IReadOnlyList<IDiff> results)
        {
            Assert.IsTrue(results.Count <= 1);
            if (results.Any())
            {
                var source = (AttrDiff)results[0];
                Assert.AreEqual("div(0) > svg(0) > g(0) > path(1)[d]", source.Test.Path);
            }
        }
    }
}
