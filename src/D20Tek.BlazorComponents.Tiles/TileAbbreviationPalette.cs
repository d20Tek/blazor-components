namespace D20Tek.BlazorComponents;

internal static class TileAbbreviationPalette
{
    private static readonly string[] _colors =
    [
        "#5b6b8c", // muted indigo
        "#6d5b8c", // muted violet
        "#8c5b7a", // muted mauve
        "#8c5b5b", // muted brick
        "#8c6f5b", // muted clay
        "#8c855b", // muted olive
        "#6f8c5b", // muted moss
        "#5b8c6a", // muted sage
        "#5b8c85", // muted teal
        "#5b7a8c", // muted steel
        "#4f6d6d", // muted slate teal
        "#6d5f4f", // muted taupe
        "#7a6d8c", // muted lavender gray
        "#8c7a6d", // muted sand
        "#5f7a6d", // muted eucalyptus
        "#736b8c", // muted periwinkle
    ];

    public static int Count => _colors.Length;

    public static string GetColor(string? key)
    {
        var index = GetIndex(key);
        return _colors[index];
    }

    private static int GetIndex(string? key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return 0;
        }

        var hash = 0;
        foreach (var c in key)
        {
            hash = unchecked((hash * 31) + c);
        }

        return Math.Abs(hash % _colors.Length);
    }
}
