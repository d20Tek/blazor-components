﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
using Bunit;
using System.Collections.Generic;
using mst = Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D20Tek.BlazorComponents.UnitTests
{
    [mst.TestClass]
    public class SpinnerTests
    {
        [mst.TestMethod]
        public void DefaultRender()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>();

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_IsVisibleFalse()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.IsVisible, false));

            // assert
            comp.MarkupMatches(string.Empty);
        }

        [mst.TestMethod]
        public void Render_PulseType()
        {
            // arrange
            var ctx = new TestContext();

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.Type, SpinType.Pulse));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner-pulse""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithAttributeSplat()
        {
            // arrange
            var ctx = new TestContext();
            var attr = new Dictionary<string, object>
            {
                { "style", "color: red; height: 120px; width 120 px" },
                { "disabled", true}
            };

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.RemainingAttributes, attr));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner"" style=""color: red; height: 120px; width 120 px"" disabled=""""></div>";
            comp.MarkupMatches(expectedHtml);
        }

        [mst.TestMethod]
        public void Render_WithAdditionalCssClasses()
        {
            // arrange
            var ctx = new TestContext();
            var attr = new Dictionary<string, object>
            {
                { "class", "test-component text-white" }
            };

            // act
            var comp = ctx.RenderComponent<Spinner>(parameters => parameters
                .Add(p => p.RemainingAttributes, attr));

            // assert
            var expectedHtml = @"<div role=""status"" class=""spinner test-component text-white""></div>";
            comp.MarkupMatches(expectedHtml);
        }
    }
}
