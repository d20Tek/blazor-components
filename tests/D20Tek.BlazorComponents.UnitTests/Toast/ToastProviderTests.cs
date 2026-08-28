namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed class ToastProviderTests
{
    private static BunitContext CreateContext(out IToastService service)
    {
        var ctx = new BunitContext();
        ctx.Services.AddToast();
        service = ctx.Services.GetRequiredService<IToastService>();
        return ctx;
    }

    [TestMethod]
    public void InitialRender_ShowsNoToasts()
    {
        // arrange
        using var ctx = CreateContext(out _);

        // act
        var comp = ctx.Render<ToastProvider>();

        // assert
        Assert.IsEmpty(comp.FindAll(".toast"));
    }

    [TestMethod]
    public void Show_RendersToastWithContentAndVariant()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.InvokeAsync(() => service.Show("hello", NotificationVariant.Success));

        // assert
        Assert.HasCount(1, comp.FindAll(".toast"));
        Assert.Contains("hello", comp.Markup);
        Assert.Contains("toast-success", comp.Markup);
        Assert.Contains("toast-host-bottom-right", comp.Markup);
    }

    [TestMethod]
    public void Show_WithShowIconFalse_HidesIcon()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.InvokeAsync(() => service.Show("no icon", o => o.ShowIcon = false));

        // assert
        Assert.IsEmpty(comp.FindAll(".toast__icon"));
    }

    [TestMethod]
    public void Show_WithShowIconTrue_RendersIcon()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.InvokeAsync(() => service.Show("icon"));

        // assert
        Assert.HasCount(1, comp.FindAll(".toast__icon"));
    }

    [TestMethod]
    public void Show_WithDismissibleFalse_HidesCloseButton()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.InvokeAsync(() => service.Show("sticky", o =>
        {
            o.Dismissible = false;
            o.Timeout = TimeSpan.Zero;
        }));

        // assert
        Assert.IsEmpty(comp.FindAll(".toast__close-btn"));
    }

    [TestMethod]
    public void CloseButton_DismissesToast()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();
        comp.InvokeAsync(() => service.Show("dismiss me", o => o.Timeout = TimeSpan.Zero));

        // act
        comp.Find(".toast__close-btn").Click();

        // assert
        Assert.IsEmpty(comp.FindAll(".toast"));
    }

    [TestMethod]
    public void ServiceDismiss_RemovesToast()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();
        ToastInstance? shown = null;
        comp.InvokeAsync(() => shown = service.Show("bye", o => o.Timeout = TimeSpan.Zero));

        // act
        comp.InvokeAsync(() => service.Dismiss(shown!.Id));

        // assert
        Assert.IsEmpty(comp.FindAll(".toast"));
    }

    [TestMethod]
    public void Show_GroupsToastsByPosition()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.InvokeAsync(() =>
        {
            service.Show("top", o => { o.Position = ToastPosition.TopLeft; o.Timeout = TimeSpan.Zero; });
            service.Show("bottom", o => { o.Position = ToastPosition.BottomRight; o.Timeout = TimeSpan.Zero; });
        });

        // assert
        Assert.HasCount(1, comp.FindAll(".toast-host-top-left"));
        Assert.HasCount(1, comp.FindAll(".toast-host-bottom-right"));
        Assert.HasCount(2, comp.FindAll(".toast"));
    }

    [TestMethod]
    public void MaxVisible_CapsToastsPerPosition()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>(p => p.Add(x => x.MaxVisible, 2));

        // act
        comp.InvokeAsync(() =>
        {
            for (var i = 0; i < 5; i++)
            {
                service.Show($"msg {i}", o => o.Timeout = TimeSpan.Zero);
            }
        });

        // assert
        Assert.HasCount(2, comp.FindAll(".toast"));
    }

    [TestMethod]
    public async Task Show_WithTimeout_AutoDismisses()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();
        await comp.InvokeAsync(() => service.Show("auto", o => o.Timeout = TimeSpan.FromMilliseconds(50)));
        Assert.HasCount(1, comp.FindAll(".toast"));

        // act
        await Task.Delay(200, CancellationToken.None);

        // assert
        comp.WaitForAssertion(() => Assert.IsEmpty(comp.FindAll(".toast")), TimeSpan.FromSeconds(2));
    }

    [TestMethod]
    public void CustomCloseButtonAriaLabel_IsApplied()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>(p => p.Add(x => x.CloseButtonAriaLabel, "Close toast"));

        // act
        comp.InvokeAsync(() => service.Show("labeled", o => o.Timeout = TimeSpan.Zero));

        // assert
        Assert.Contains("aria-label=\"Close toast\"", comp.Markup);
    }

    [TestMethod]
    public void Dispose_UnsubscribesFromService()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();

        // act
        comp.Instance.Dispose();
        comp.InvokeAsync(() => service.Show("after dispose", o => o.Timeout = TimeSpan.Zero));

        // assert - toast not added since host is disposed/unsubscribed
        Assert.IsEmpty(comp.FindAll(".toast"));
    }
}
