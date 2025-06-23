using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Portfolio.Core
{
    public static class ModuleCoreDependencies
    {

        public static IServiceCollection AddCoreDependencies(this IServiceCollection Service)
        {
            // Mediator
            Service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return Service;
        }
    }
}
