using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Company.Framework.Mediator.Extensions;

public class MediatorServiceBuilder
{
    public IServiceCollection Services { get; }

    private bool _isBuilt;

    public MediatorServiceBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Build()
    {
        if (_isBuilt)
            throw new InvalidOperationException("Mediator services built before");
        _isBuilt = true;
        return Services;
    }

    public MediatorServiceBuilder WithPreProcessor<TProcessor>()
    {
        return WithPreProcessor(typeof(TProcessor));
    }

    public MediatorServiceBuilder WithPreProcessor(Type processorType)
    {
        Services.TryAddScoped(typeof(IRequestPreProcessor<>), processorType);
        return this;
    }

    public MediatorServiceBuilder WithPostProcessor<TProcessor>()
    {
        return WithPostProcessor(typeof(TProcessor));
    }

    public MediatorServiceBuilder WithPostProcessor(Type processorType)
    {
        Services.TryAddScoped(typeof(IRequestPostProcessor<,>), processorType);
        return this;
    }
}