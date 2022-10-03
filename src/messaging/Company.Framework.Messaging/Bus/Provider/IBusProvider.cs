using Company.Framework.Messaging.Bus;

namespace Company.Framework.Messaging.Producer.Context.Provider
{
    public interface IBusProvider
    {
        TBus Resolve<TBus>(string name) where TBus : IBus;
    }
}
