//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) d20Tek. All rights reserved.
//---------------------------------------------------------------------------------------------------------------------

using System.Text;

namespace D20Tek.BlazorComponents.Utilities
{
    public class StyleBuilder
    {
        private const string _attributNameStyle = "style";
        private readonly StringBuilder _stringBuilder;

        public StyleBuilder(string property, string value)
            : this()
        {
            this.AddStyle(property, value);
        }

        public StyleBuilder()
        {
            this._stringBuilder = new StringBuilder();
        }

        public StyleBuilder AddStyle(string property, string value)
        {
            if (string.IsNullOrWhiteSpace(property)) throw new ArgumentNullException(nameof(property));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

            return this.AddValue($"{property}: {value}; ");
        }

        public StyleBuilder AddStyle(string property, string value, bool condition = true) =>
            condition ? this.AddStyle(property, value) : this;

        public StyleBuilder AddStyle(string property, string value, Func<bool> condition) =>
            this.AddStyle(property, value, condition());

        public StyleBuilder AddStyleFromAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null)
                return this;

            attributes.TryGetValue(_attributNameStyle, out var value);
            var text = value?.ToString();
            return (text is null) ? this : this.AddValue($"{text}; ");
        }

        public string? Build() =>
            this._stringBuilder.ToString().Trim().NullIfEmpty();

        public override string? ToString() => Build();

        private StyleBuilder AddValue(string style)
        {
            this._stringBuilder.Append(style);
            return this;
        }
    }
}
