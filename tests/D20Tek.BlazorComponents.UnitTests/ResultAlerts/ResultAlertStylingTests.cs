namespace D20Tek.BlazorComponents.UnitTests.ResultAlerts;

public sealed partial class ResultAlertTests
{
    [TestMethod]
    public void Render_VariantOverride_OverridesAutoVariant()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Variant, AlertVariant.Warning));

        // assert
        Assert.Contains("result-alert-warning", cut.Markup);
        Assert.DoesNotContain("result-alert-success", cut.Markup);
    }

    [TestMethod]
    public void Render_SuccessVariantOverride_AppliesOnSuccess()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.SuccessVariant, AlertVariant.Info));

        // assert
        Assert.Contains("result-alert-info", cut.Markup);
    }

    [TestMethod]
    public void Render_FailureVariantOverride_AppliesOnFailure()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, FailureResult())
            .Add(p => p.FailureVariant, AlertVariant.Neutral));

        // assert
        Assert.Contains("result-alert-neutral", cut.Markup);
    }

    [TestMethod]
    public void Render_SizeAndModifiers_ProduceCssClasses()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Size, Size.Large)
            .Add(p => p.Bordered, true)
            .Add(p => p.Elevated, true));

        // assert
        Assert.Contains("result-alert-lg", cut.Markup);
        Assert.Contains("result-alert-bordered", cut.Markup);
        Assert.Contains("result-alert-elevated", cut.Markup);
    }

    [TestMethod]
    public void Render_AnimateDefault_ProducesAnimatedClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult()));

        // assert
        Assert.Contains("result-alert-animated", cut.Markup);
    }

    [TestMethod]
    public void Render_AnimateFalse_OmitsAnimatedClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.Animate, false));

        // assert
        Assert.DoesNotContain("result-alert-animated", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowIconFalse_HidesIcon()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowIcon, false));

        // assert
        Assert.IsEmpty(cut.FindAll(".result-alert__icon"));
    }

    [TestMethod]
    public void Render_RemainingAttributes_MergesClass()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .AddUnmatched("class", "extra-class"));

        // assert
        Assert.Contains("extra-class", cut.Markup);
        Assert.Contains("result-alert", cut.Markup);
    }

    [TestMethod]
    public void Render_ShowCloseButton_RendersButtonAndDismissFiresCallback()
    {
        // arrange
        var dismissed = false;
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowCloseButton, true)
            .Add(p => p.OnDismiss, () => dismissed = true));

        // act
        cut.Find(".result-alert__close-btn").Click();

        // assert
        Assert.IsTrue(dismissed);
        Assert.AreEqual(string.Empty, cut.Markup.Trim());
    }

    [TestMethod]
    public void Render_CustomCloseButtonAriaLabel_IsApplied()
    {
        // arrange - act
        var cut = Render<ResultAlert<TestModel>>(parameters => parameters
            .Add(p => p.Result, SuccessResult())
            .Add(p => p.ShowCloseButton, true)
            .Add(p => p.CloseButtonAriaLabel, "Close alert"));

        // assert
        Assert.Contains("aria-label=\"Close alert\"", cut.Markup);
    }
}
