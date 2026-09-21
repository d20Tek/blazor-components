namespace D20Tek.BlazorComponents.UnitTests.Toast;

public sealed partial class ToastProviderTests
{
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
        Assert.IsEmpty(comp.FindAll(".d20tek-toast"));
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
        Assert.IsEmpty(comp.FindAll(".d20tek-toast"));
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
        Assert.HasCount(2, comp.FindAll(".d20tek-toast"));
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
        Assert.HasCount(2, comp.FindAll(".d20tek-toast"));
    }

    [TestMethod]
    public async Task Show_WithTimeout_AutoDismisses()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();
        await comp.InvokeAsync(() => service.Show("auto", o => o.Timeout = TimeSpan.FromMilliseconds(50)));
        Assert.HasCount(1, comp.FindAll(".d20tek-toast"));

        // act
        await Task.Delay(200, CancellationToken.None);

        // assert
        comp.WaitForAssertion(() => Assert.IsEmpty(comp.FindAll(".d20tek-toast")), TimeSpan.FromSeconds(2));
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
        Assert.IsEmpty(comp.FindAll(".d20tek-toast"));
    }

    [TestMethod]
    public void HandleShow_AfterDispose_DoesNotAddToast()
    {
        // arrange
        using var ctx = CreateContext(out _);
        var comp = ctx.Render<ToastProvider>();
        comp.Instance.Dispose();
        var toast = new ToastInstance 
        { 
            Content = [ExcludeFromCodeCoverage](builder) => builder.AddContent(0, "late"),
            Timeout = TimeSpan.Zero
        };

        // act - direct invocation simulates an in-flight callback racing disposal
        comp.InvokeAsync(() => ToastProviderAccessor.HandleShow(comp.Instance, toast));

        // assert - guard short-circuits so nothing renders
        Assert.IsEmpty(comp.FindAll(".d20tek-toast"));
    }

    [TestMethod]
    public void RemoveToast_AfterDispose_ShortCircuits()
    {
        // arrange
        using var ctx = CreateContext(out var service);
        var comp = ctx.Render<ToastProvider>();
        ToastInstance? shown = null;
        comp.InvokeAsync(() => shown = service.Show("sticky", o => o.Timeout = TimeSpan.Zero));
        comp.Instance.Dispose();

        // act - a pending timer callback could call RemoveToast after dispose
        comp.InvokeAsync(() => ToastProviderAccessor.RemoveToast(comp.Instance, shown!.Id));

        // assert - guard prevents a post-dispose render; markup is unchanged
        Assert.HasCount(1, comp.FindAll(".d20tek-toast"));
    }
}
