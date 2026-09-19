namespace D20Tek.BlazorComponents;

internal static class TileAbbreviation
{
    private static readonly char[] _separators = [' ', '\t', '\r', '\n'];

    public static string Compute(string? title, string? description, string? overrideValue) =>
        (overrideValue, title, description) switch
        {
            _ when !string.IsNullOrWhiteSpace(overrideValue) => Normalize(overrideValue.Trim()),
            _ when !string.IsNullOrWhiteSpace(title) => FromWords(title),
            _ when !string.IsNullOrWhiteSpace(description) => FromWords(description),
            _ => "?",
        };

    private static string FromWords(string value)
    {
        var words = value.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
        char[] initials = [.. words.Take(2).Select(w => w[0])];

        return new string(initials).ToUpperInvariant();
    }

    private static string Normalize(string value) =>
        value.Length <= 2 ? value.ToUpperInvariant() : value[..2].ToUpperInvariant();
}
