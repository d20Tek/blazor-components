namespace D20Tek.BlazorComponents.UnitTests.Toast;

public sealed partial class ToastServiceTests
{
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
