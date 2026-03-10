using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.BlazorComponents;

public static class DependencyInjection
{
    public static IServiceCollection AddMessageBox(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBoxService, MessageBoxService>();
        return services;
    }
}
