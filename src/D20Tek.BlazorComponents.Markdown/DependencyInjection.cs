using Microsoft.Extensions.DependencyInjection;

namespace D20Tek.BlazorComponents;

public static class DependencyInjection
{
    public static IServiceCollection AddMarkdownRenderer(this IServiceCollection services) =>
        services.AddSingleton<IMarkdownRenderer, MarkdigRenderer>();
}
