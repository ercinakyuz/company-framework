namespace Company.Framework.Messaging.Producer.Context.Provider;

public class ProducerContextProvider : IProducerContextProvider
{
    private readonly Func<Type, IProducerContext> _productContextProvision;
    
    public ProducerContextProvider(Func<Type, IProducerContext> productContextProvision)
    {
        _productContextProvision = productContextProvision;
    }

    public TContext Resolve<TContext>() where TContext : IProducerContext
    {
        return (TContext)Resolve(typeof(TContext));
    }

    public IProducerContext Resolve(Type contextType)
    {
        return _productContextProvision(contextType);
    }
}