using System.Text;

namespace D20Tek.BlazorComponents.Utilities;

public class StyleBuilder
{
    private const string _attributNameStyle = "style";
    private readonly StringBuilder _stringBuilder;

    public StyleBuilder(string property, string value) : this() => AddStyle(property, value);

    public StyleBuilder() => _stringBuilder = new StringBuilder();

    public StyleBuilder AddStyle(string property, string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(property);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);

        return AddValue($"{property}: {value}; ");
    }

    public StyleBuilder AddStyle(string property, string value, bool condition = true) =>
        condition ? AddStyle(property, value) : this;

    public StyleBuilder AddStyle(string property, string value, Func<bool> condition) => 
        AddStyle(property, value, condition());

    public StyleBuilder AddStyleFromAttributes(IDictionary<string, object> attributes)
    {
        if (attributes == null) return this;

        attributes.TryGetValue(_attributNameStyle, out var value);
        var text = value?.ToString();
        return (text is null) ? this : AddValue($"{text}; ");
    }

    public string? Build() => _stringBuilder.ToString().Trim().NullIfEmpty();

    public override string? ToString() => Build();

    private StyleBuilder AddValue(string style)
    {
        _stringBuilder.Append(style);
        return this;
    }
}
