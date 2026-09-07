namespace D20Tek.BlazorComponents.UnitTests.Result;

public sealed partial class ResultToastExtensionsTests
{
    [TestMethod]
    public void ShowResult_Success_SeedsPresentationFromDefaults()
    {
        // arrange
        var service = new FakeToastService
        {
            Defaults = new ToastDefaults
            {
                Position = ToastPosition.TopLeft,
                DefaultTimeout = TimeSpan.FromSeconds(9),
                ShowIcon = false,
                Dismissible = false,
                Animate = false,
            },
        };

        // act
        service.ShowResult(SuccessResult());

        // assert
        var toast = service.LastToast!;
        Assert.AreEqual(ToastPosition.TopLeft, toast.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(9), toast.Timeout);
        Assert.IsFalse(toast.ShowIcon);
        Assert.IsFalse(toast.Dismissible);
        Assert.IsFalse(toast.Animate);
    }

    [TestMethod]
    public void ShowResult_Failure_SeedsPositionFromDefaultsButStaysSticky()
    {
        // arrange
        var service = new FakeToastService
        {
            Defaults = new ToastDefaults
            {
                Position = ToastPosition.TopRight,
                DefaultTimeout = TimeSpan.FromSeconds(9),
            },
        };

        // act
        service.ShowResult(FailureResult());

        // assert
        var toast = service.LastToast!;
        Assert.AreEqual(ToastPosition.TopRight, toast.Position);
        Assert.IsTrue(toast.IsSticky);
    }

    [TestMethod]
    public void ShowResult_Configure_OverridesDefaults()
    {
        // arrange
        var service = new FakeToastService
        {
            Defaults = new ToastDefaults
            {
                Position = ToastPosition.TopLeft,
                DefaultTimeout = TimeSpan.FromSeconds(9),
            },
        };

        // act
        service.ShowResult(SuccessResult(), o =>
        {
            o.Position = ToastPosition.BottomCenter;
            o.SuccessTimeout = TimeSpan.FromSeconds(2);
        });

        // assert
        var toast = service.LastToast!;
        Assert.AreEqual(ToastPosition.BottomCenter, toast.Position);
        Assert.AreEqual(TimeSpan.FromSeconds(2), toast.Timeout);
    }
}
