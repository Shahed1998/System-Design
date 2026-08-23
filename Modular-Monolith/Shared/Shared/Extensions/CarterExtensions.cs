using Carter;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Extensions
{
    public static class CarterExtensions
    {
        public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddCarter(configurator: config =>
            {
                var modules = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsAssignableTo(typeof(ICarterModule)) && !t.IsAbstract && !t.IsInterface)
                .ToArray();

                config.WithModules(modules);

            });

            return services;
        }
    }
}
