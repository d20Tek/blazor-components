namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastServiceTests
{
    private static readonly RenderFragment _content = [ExcludeFromCodeCoverage](builder) => builder.AddContent(0, "hi");

    [TestMethod]
    public void Show_WithFragment_RaisesOnShowWithDefaults()
    {
        // arrange
        var service = new ToastService();
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        var result = service.Show(_content);

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(result.Id, captured.Id);
        Assert.AreEqual(NotificationVariant.Info, captured.Variant);
        Assert.AreEqual(ToastPosition.BottomRight, captured.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(5), captured.Timeout);
        Assert.IsTrue(captured.ShowIcon);
        Assert.IsTrue(captured.Dismissible);
        Assert.IsTrue(captured.Animate);
        Assert.IsFalse(captured.IsSticky);
    }

    [TestMethod]
    public void Show_WithFragment_AppliesConfigure()
    {
        // arrange
        var service = new ToastService();
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show(_content, o =>
        {
            o.Variant = NotificationVariant.Error;
            o.Position = ToastPosition.TopLeft;
            o.Timeout = TimeSpan.Zero;
            o.ShowIcon = false;
            o.Dismissible = false;
            o.Animate = false;
        });

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(NotificationVariant.Error, captured.Variant);
        Assert.AreEqual(ToastPosition.TopLeft, captured.Position);
        Assert.AreEqual(TimeSpan.Zero, captured.Timeout);
        Assert.IsFalse(captured.ShowIcon);
        Assert.IsFalse(captured.Dismissible);
        Assert.IsFalse(captured.Animate);
        Assert.IsTrue(captured.IsSticky);
    }

    [TestMethod]
    public void Show_WithNullContent_Throws()
    {
        // arrange
        var service = new ToastService();

        // act - assert
        Assert.ThrowsExactly<ArgumentNullException>([ExcludeFromCodeCoverage]() => service.Show((RenderFragment)null!));
    }

    [TestMethod]
    public void Show_WithMessage_RendersMessageContent()
    {
        // arrange
        using var ctx = new BunitContext();
        var service = new ToastService();
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show("hello world");

        // assert
        Assert.IsNotNull(captured);
        var rendered = ctx.Render(captured.Content);
        Assert.Contains("hello world", rendered.Markup);
    }

    [TestMethod]
    public void Show_WithMessageAndVariant_SetsVariant()
    {
        // arrange
        var service = new ToastService();
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show("boom", NotificationVariant.Warning);

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(NotificationVariant.Warning, captured.Variant);
    }

    [TestMethod]
    public void Show_WithMessageVariantAndConfigure_ConfigureOverridesPosition()
    {
        // arrange
        var service = new ToastService();
        ToastInstance? captured = null;
        service.OnShow += t => captured = t;

        // act
        service.Show("boom", NotificationVariant.Success, o => o.Position = ToastPosition.TopCenter);

        // assert
        Assert.IsNotNull(captured);
        Assert.AreEqual(NotificationVariant.Success, captured.Variant);
        Assert.AreEqual(ToastPosition.TopCenter, captured.Position);
    }

    [TestMethod]
    public void Dismiss_RaisesOnDismissWithId()
    {
        // arrange
        var service = new ToastService();
        Guid? captured = null;
        service.OnDismiss += id => captured = id;
        var id = Guid.NewGuid();

        // act
        service.Dismiss(id);

        // assert
        Assert.AreEqual(id, captured);
    }

    [TestMethod]
    public void Show_WithNoSubscribers_DoesNotThrow()
    {
        // arrange
        var service = new ToastService();

        // act
        var result = service.Show("safe");

        // assert
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Dismiss_WithNoSubscribers_DoesNotThrow()
    {
        // arrange
        var service = new ToastService();

        // act - assert (no OnDismiss subscribers)
        service.Dismiss(Guid.NewGuid());
    }
}
