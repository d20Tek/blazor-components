using System.Reflection;

namespace D20Tek.BlazorComponents.UnitTests.Toast;

internal static class ToastProviderAccessor
{
    private const BindingFlags InstanceNonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    public static void HandleShow(ToastProvider provider, ToastInstance toast)
    {
        var method = typeof(ToastProvider).GetMethod("HandleShow", InstanceNonPublic);
        method!.Invoke(provider, [toast]);
    }

    public static void RemoveToast(ToastProvider provider, Guid id)
    {
        var method = typeof(ToastProvider).GetMethod("RemoveToast", InstanceNonPublic);
        method!.Invoke(provider, [id]);
    }
}
