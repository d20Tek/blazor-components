namespace D20Tek.BlazorComponents.Selectors;

internal sealed class ErrorMapper(ValidationMessageStore messageStore, EditContext editContext)
{
    public void Map(IEnumerable<Error> errors, IErrorFieldSelector selector)
    {
        var context = new FieldSelectorContext(editContext, editContext.Model!.GetType());

        foreach (var error in errors)
        {
            var produced = false;
            foreach (var fieldName in selector.GetFieldNames(error, context))
            {
                produced = true;
                messageStore.Add(editContext.Field(fieldName), error.Message);
            }

            if (!produced)
            {
                messageStore.Add(editContext.Field(IErrorFieldSelector.FormLevelField), error.Message);
            }
        }
    }
}
