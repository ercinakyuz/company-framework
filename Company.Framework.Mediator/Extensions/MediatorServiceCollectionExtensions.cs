using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Mediator.Extensions
{
    public static class MediatorServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            return services.AddMediatR(Assembly.GetCallingAssembly());
        }
    }
}