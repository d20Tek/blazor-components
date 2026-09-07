namespace D20Tek.BlazorComponents;

public static class DependencyInjection
{
    public static IServiceCollection AddToast(this IServiceCollection services, Action<ToastDefaults>? configure = null)
    {
        var defaults = new ToastDefaults();
        configure?.Invoke(defaults);

        return services.AddSingleton(defaults)
                       .AddScoped<IToastService, ToastService>();
    }
}
