using GraphiteUi.Components;
using Microsoft.Extensions.DependencyInjection;

namespace GraphiteUI.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the GraphiteUI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGraphiteUi(this IServiceCollection services)
    {
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IToastService, ToastService>();

        return services;
    }
}
