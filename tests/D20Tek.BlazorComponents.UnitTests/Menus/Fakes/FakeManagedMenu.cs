namespace D20Tek.BlazorComponents.UnitTests.Menus.Fakes;

internal sealed class FakeManagedMenu : BlazorComponents.IManagedMenu
{
    public int CloseRequestCount { get; private set; }

    public Task RequestCloseAsync()
    {
        CloseRequestCount++;
        return Task.CompletedTask;
    }
}
