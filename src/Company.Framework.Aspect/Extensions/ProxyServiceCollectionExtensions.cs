using System.Reflection;
using Company.Framework.Aspect.Processors;
using Company.Framework.Aspect.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Aspect.Extensions
{
    public static class ProxyServiceCollectionExtensions
    {
        public static IServiceCollection AddProxiedComponent<TAbstraction, TImplementation, TPreProcessor, TPostProcessor>(this IServiceCollection serviceCollection)
            where TImplementation : class, TAbstraction
            where TAbstraction : class
            where TPreProcessor : IPreProcessor
            where TPostProcessor : IPostProcessor
        {
            serviceCollection
                .AddSingleton<TImplementation>()
                .AddSingleton(serviceProvider =>
                {
                    object proxy = DispatchProxy.Create<TAbstraction, CoreProxy<TAbstraction>>();
                    var coreProxy = (CoreProxy<TAbstraction>)proxy;
                    coreProxy.SetDecoration(serviceProvider.GetRequiredService<TImplementation>());
                    coreProxy.SetPreProcessors(serviceProvider.GetServices<TPreProcessor>());
                    coreProxy.SetPostProcessors(serviceProvider.GetServices<TPostProcessor>());
                    return (TAbstraction)proxy;
                });

            return serviceCollection;
        }
    }
}
