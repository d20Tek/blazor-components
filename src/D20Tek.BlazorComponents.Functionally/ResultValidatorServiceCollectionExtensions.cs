namespace D20Tek.BlazorComponents;

public static class ResultValidatorServiceCollectionExtensions
{
    public static IServiceCollection AddResultValidator(this IServiceCollection services, Action<ResultValidatorOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var options = new ResultValidatorOptions();
        configure?.Invoke(options);

        services.AddSingleton(options);
        services.AddSingleton<IErrorFieldSelector>(sp => sp.GetRequiredService<ResultValidatorOptions>().FieldSelector);
        return services;
    }
}
