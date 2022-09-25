namespace Company.Framework.Messaging.Producer.Context.Provider;

public interface IProducerContextProvider
{
    TContext Resolve<TContext>() where TContext : IProducerContext;
}