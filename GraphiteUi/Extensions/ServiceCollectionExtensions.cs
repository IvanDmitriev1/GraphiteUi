using GraphiteUi.Components;
using Microsoft.Extensions.DependencyInjection;
using TailwindMerge;

namespace GraphiteUI.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the GraphiteUI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="options">An action to configure the <see cref="TwConfig"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGraphiteUi(this IServiceCollection services, Action<TwConfig>? options = null)
    {
        if (options is null)
        {
            services.AddTwMerge();
        }
        else
        {
            services.AddTwMerge(options);
        }

        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IToastService, ToastService>();

        return services;
    }
}
