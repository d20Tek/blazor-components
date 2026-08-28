namespace D20Tek.BlazorComponents;

public static class DependencyInjection
{
    public static IServiceCollection AddToast(this IServiceCollection services) =>
        services.AddScoped<IToastService, ToastService>();
}
