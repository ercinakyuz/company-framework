using Company.Framework.Aspect.Proxy;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Aspect.Extensions;

public static class ProxyServiceCollectionExtensions
{
    public static ProxyServiceBuilder<TAbstraction> ProxyServiceBuilder<TAbstraction>(this IServiceCollection serviceCollection)
         where TAbstraction : class
    {
        return new ProxyServiceBuilder<TAbstraction>(serviceCollection);
    }
}

