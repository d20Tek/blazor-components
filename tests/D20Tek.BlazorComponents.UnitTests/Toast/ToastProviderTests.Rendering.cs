namespace D20Tek.BlazorComponents.UnitTests.Toast;

[TestClass]
public sealed partial class ToastProviderTests
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
        Assert.IsEmpty(comp.FindAll(".d20tek-toast"));
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
        Assert.HasCount(1, comp.FindAll(".d20tek-toast"));
        Assert.Contains("hello", comp.Markup);
        Assert.Contains("d20tek-toast-success", comp.Markup);
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
}
