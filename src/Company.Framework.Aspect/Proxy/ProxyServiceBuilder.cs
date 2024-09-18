using System.Reflection;
using Company.Framework.Aspect.Processors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Framework.Aspect.Proxy;

public class ProxyServiceBuilder<TAbstraction> where TAbstraction : class
{
    private readonly IServiceCollection _serviceCollection;

    private readonly ISet<Type> _preProcessorTypes;

    private readonly ISet<Type> _postProcessorTypes;

    public ProxyServiceBuilder(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
        _preProcessorTypes = new HashSet<Type>();
        _postProcessorTypes = new HashSet<Type>();
    }

    public IServiceCollection Build<TImplementation>() where TImplementation : class, TAbstraction
    {
        return _serviceCollection.AddSingleton<TImplementation>()
            .AddSingleton(serviceProvider =>
            {
                object proxy = DispatchProxy.Create<TAbstraction, CoreProxy<TAbstraction>>();
                var coreProxy = (CoreProxy<TAbstraction>)proxy;
                coreProxy.SetDecoration(serviceProvider.GetRequiredService<TImplementation>());
                AddPreProcessors(serviceProvider, coreProxy);
                AddPostProcessors(serviceProvider, coreProxy);
                return (TAbstraction)proxy;
            });
    }

    public ProxyServiceBuilder<TAbstraction> WithPreProcessor<TPreProcessor>() where TPreProcessor : class, IPreProcessor
    {
        _preProcessorTypes.Add(typeof(TPreProcessor));
        return this;
    }

    public ProxyServiceBuilder<TAbstraction> WithPostProcessor<TPostProcessor>() where TPostProcessor : class, IPostProcessor
    {
        _postProcessorTypes.Add(typeof(TPostProcessor));
        return this;
    }

    private void AddPreProcessors(IServiceProvider serviceProvider, CoreProxy<TAbstraction> proxy)
    {
        foreach (var _preProcessorType in _preProcessorTypes)
        {
            var processor = (IPreProcessor)serviceProvider.GetRequiredService(_preProcessorType);
            proxy.AddPreProcessor(processor);
        }
    }

    private void AddPostProcessors(IServiceProvider serviceProvider, CoreProxy<TAbstraction> proxy)
    {
        foreach (var _postProcessorType in _postProcessorTypes)
        {
            var processor = (IPostProcessor)serviceProvider.GetRequiredService(_postProcessorType);
            proxy.AddPostProcessor(processor);
        }
    }


}

