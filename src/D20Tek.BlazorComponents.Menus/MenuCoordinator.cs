namespace D20Tek.BlazorComponents;

internal static class MenuCoordinator
{
    private static readonly Dictionary<string, IManagedMenu> _openByGroup = [];
    private static readonly Lock _sync = new();

    public static async Task NotifyOpenedAsync(string group, IManagedMenu menu)
    {
        IManagedMenu? previous;
        lock (_sync)
        {
            _openByGroup.TryGetValue(group, out previous);
            _openByGroup[group] = menu;
        }

        if (previous is not null && !ReferenceEquals(previous, menu))
        {
            await previous.RequestCloseAsync();
        }
    }

    public static void NotifyClosed(string group, IManagedMenu menu)
    {
        lock (_sync)
        {
            if (_openByGroup.TryGetValue(group, out var current) &&
                ReferenceEquals(current, menu))
            {
                _openByGroup.Remove(group);
            }
        }
    }
}
