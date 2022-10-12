namespace Company.Framework.Messaging.Bus.Provider
{
    public interface IBusProvider
    {
        TBus Resolve<TBus>(string name) where TBus : IBus;
    }
}
