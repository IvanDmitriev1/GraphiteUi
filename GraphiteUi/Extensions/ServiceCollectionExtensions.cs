using GraphiteUi.Components;
using GraphiteUi.Components.Dialog;
using Microsoft.Extensions.DependencyInjection;
using TailwindMerge;

namespace GraphiteUI.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the GraphiteUI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="options">An action to configure the <see cref="TwMergeConfig"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGraphiteUi(this IServiceCollection services, Action<TwConfig>? options = null)
    {
        services.AddTwMerge(config =>
        {
            config.CacheSize(1);
        });

        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IToastService, ToastService>();

        if (options is not null)
        {
            services.Configure(options);
        }

        return services;
    }
}
