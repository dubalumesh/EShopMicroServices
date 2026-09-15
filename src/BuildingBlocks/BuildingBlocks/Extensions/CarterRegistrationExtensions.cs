
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Carter;

namespace BuildingBlocks.Extensions
{
    public static class CarterRegistrationExtensions
    {
        public static IServiceCollection AddCarterModulesFromAssembly(
           this IServiceCollection services,
           Assembly assembly)
        {
            services.AddCarter(configurator: config =>
            {
                var modules = assembly.GetTypes()
                    .Where(t =>
                        !t.IsAbstract &&
                        typeof(ICarterModule).IsAssignableFrom(t));

                var withModuleMethod = config.GetType()
                    .GetMethods()
                    .First(m =>
                        m.Name == nameof(config.WithModule) &&
                        m.IsGenericMethod);

                foreach (var module in modules)
                {
                    withModuleMethod
                        .MakeGenericMethod(module)
                        .Invoke(config, null);
                }
            });

            return services;
        }
    }
}
