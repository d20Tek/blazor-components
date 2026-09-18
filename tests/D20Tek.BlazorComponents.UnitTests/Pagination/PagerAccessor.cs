using System.Reflection;

namespace D20Tek.BlazorComponents.UnitTests.Pagination;

internal static class PagerAccessor
{
    private const BindingFlags InstanceNonPublic = BindingFlags.NonPublic | BindingFlags.Instance;

    public static void SetPageSize(Pager pager, int value)
    {
        var field = typeof(Pager).GetField("_pageSize", InstanceNonPublic);
        field!.SetValue(pager, value);
    }
}
