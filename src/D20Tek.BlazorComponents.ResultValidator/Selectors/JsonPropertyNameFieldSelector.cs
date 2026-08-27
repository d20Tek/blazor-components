using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;

namespace D20Tek.BlazorComponents.Selectors;

public sealed class JsonPropertyNameFieldSelector : IErrorFieldSelector
{
    private static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _cache = new();

    public IEnumerable<string> GetFieldNames(Error error, FieldSelectorContext context)
    {
        if (error.Type is not ErrorType.Validation)
        {
            yield return IErrorFieldSelector.FormLevelField;
            yield break;
        }

        var map = context.ModelType is null ? null : GetMap(context.ModelType);
        if (map is not null && map.TryGetValue(error.Code, out var propertyName))
        {
            yield return propertyName;
            yield break;
        }

        yield return error.Code;
    }

    private static Dictionary<string, string> GetMap(Type modelType) =>
        _cache.GetOrAdd(modelType, static t =>
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var jsonName = prop.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name;
                if (!string.IsNullOrWhiteSpace(jsonName))
                {
                    map[jsonName] = prop.Name;
                }
            }
            return map;
        });
}
