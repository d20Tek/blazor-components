using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace D20Tek.BlazorComponents.Selectors;

public sealed class DisplayNameFieldSelector : IErrorFieldSelector
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
                var display = prop.GetCustomAttribute<DisplayAttribute>()?.GetName();
                if (!string.IsNullOrWhiteSpace(display))
                {
                    map[display] = prop.Name;
                }
            }
            return map;
        });
}
