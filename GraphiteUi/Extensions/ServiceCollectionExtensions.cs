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
        services.AddTwMerge(config =>
        {
            // Graphite typography must be treated as font sizes, not text colors.
            config.ClassGroups(new Dictionary<string, List<object>>
            {
                ["font-size"] =
                [
                    new Dictionary<string, List<object>>
                    {
                        ["text"] = ["h1", "h2", "h3", "h4", "h5", "h6", "body", "body-sm", "body-lg",
                            "caption", "heading", "regular", "medium", "large", "small", "header", "label-large", "label-small"]
                    }
                ]
            }, extend: true);
            options?.Invoke(config);
        });

        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IToastService, ToastService>();

        return services;
    }
}
