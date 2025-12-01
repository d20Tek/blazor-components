namespace D20Tek.BlazorComponents.Utilities;

public static class StringExtensions
{
    public static string? NullIfEmpty(this string s) => string.IsNullOrWhiteSpace(s) ? null : s;

    public static void ThrowWhenEmpty(this string target, string argumentName)
    {
        if (string.IsNullOrWhiteSpace(target))
        {
            throw new ArgumentNullException(argumentName);
        }
    }
}
