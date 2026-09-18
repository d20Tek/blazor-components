namespace D20Tek.BlazorComponents;

internal static class TileAbbreviation
{
    private static readonly char[] _separators = [' ', '\t', '\r', '\n'];

    public static string Compute(string? title, string? description, string? overrideValue)
    {
        if (!string.IsNullOrWhiteSpace(overrideValue))
        {
            return Normalize(overrideValue.Trim());
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            return FromTitle(title);
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            return description.Trim()[..1].ToUpperInvariant();
        }

        return "?";
    }

    private static string FromTitle(string title)
    {
        var words = title.Split(_separators, StringSplitOptions.RemoveEmptyEntries);

        var initials = words
            .Take(2)
            .Select(w => w[0])
            .ToArray();

        return new string(initials).ToUpperInvariant();
    }

    private static string Normalize(string value) =>
        value.Length <= 2 ? value.ToUpperInvariant() : value[..2].ToUpperInvariant();
}
