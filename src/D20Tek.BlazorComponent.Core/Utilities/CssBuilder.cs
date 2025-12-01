using System.Text;

namespace D20Tek.BlazorComponents.Utilities;

public class CssBuilder
{
    private const string _attributNameClass = "class";
    private readonly StringBuilder _stringBuilder;

    public CssBuilder(string value) => _stringBuilder = new StringBuilder(value);

    public CssBuilder() => _stringBuilder = new StringBuilder();

    public CssBuilder AddClass(string value) => AddValue($" {value}");

    public CssBuilder AddClass(string value, bool condition = true) => condition ? AddClass(value) : this;

    public CssBuilder AddClass(string value, Func<bool> condition) => AddClass(value, condition());

    public CssBuilder AddClassFromAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null) return this;

        attributes.TryGetValue(_attributNameClass, out var value);
        var text = value?.ToString();
        return (text is null) ? this : AddClass(text);
    }

    public string? Build() => _stringBuilder.ToString().Trim().NullIfEmpty();

    public override string? ToString() => Build();

    private CssBuilder AddValue(string value)
    {
        _stringBuilder.Append(value);
        return this;
    }
}
