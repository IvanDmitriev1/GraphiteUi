using GraphiteUi.Components;
using Microsoft.Extensions.DependencyInjection;
using TailwindMerge;
using TailwindMerge.Extensions;
using TailwindMerge.Models;

namespace GraphiteUI.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the GraphiteUI services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="options">An action to configure the <see cref="TwMergeConfig"/>.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGraphiteUi(this IServiceCollection services, Action<TwMergeConfig>? options = null)
    {
        services.AddTailwindMerge(config =>
        {
            config.Extend(new ExtendedConfig
            {
                Theme = new Dictionary<string, object[]>
                {
                    ["text"] = ["h1", "h2", "h3", "h4", "h5", "h6", "body", "body-sm", "body-lg",
                        "caption", "heading", "regular", "medium", "large", "small", "header", "label-large", "label-small"]
                }
            });
            options?.Invoke(config);
        });

        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IToastService, ToastService>();

        return services;
    }
}
