namespace D20Tek.BlazorComponents.Selectors;

internal sealed class ErrorMapper(ValidationMessageStore messageStore, EditContext editContext)
{
    public void Map(
        IEnumerable<Error> errors,
        IErrorFieldSelector selector,
        UnknownFieldBehavior behavior,
        IReadOnlySet<string>? knownFields = null)
    {
        var context = new FieldSelectorContext(editContext, editContext.Model!.GetType(), knownFields);

        foreach (var error in errors)
        {
            var produced = false;
            foreach (var rawFieldName in selector.GetFieldNames(error, context))
            {
                produced = true;
                var fieldName = ApplyUnknownBehavior(rawFieldName, context, behavior, error);
                if (fieldName is null) continue;
                messageStore.Add(editContext.Field(fieldName), error.Message);
            }

            if (!produced)
            {
                messageStore.Add(editContext.Field(IErrorFieldSelector.FormLevelField), error.Message);
            }
        }
    }

    private static string? ApplyUnknownBehavior(
        string rawFieldName,
        FieldSelectorContext context,
        UnknownFieldBehavior behavior,
        Error error)
    {
        if (context.KnownFields is null || context.KnownFields.Count == 0)
            return rawFieldName;

        if (string.IsNullOrEmpty(rawFieldName) || context.KnownFields.Contains(rawFieldName))
            return rawFieldName;

        return behavior switch
        {
            UnknownFieldBehavior.PassThrough => rawFieldName,
            UnknownFieldBehavior.SendToSummary => IErrorFieldSelector.FormLevelField,
            UnknownFieldBehavior.Drop => null,
            UnknownFieldBehavior.Throw =>
                throw new InvalidOperationException(
                    $"Field '{rawFieldName}' returned by the error field selector is not registered on the EditContext " +
                    $"(error code '{error.Code}')."),
            _ => rawFieldName,
        };
    }
}
