namespace Company.Framework.Messaging.Bus.Provider
{
    public class BusProvider : IBusProvider
    {
        private readonly IDictionary<string, IBus> _busDictionary;

        public BusProvider(IEnumerable<IBus> buses) 
        {
            _busDictionary = buses.ToDictionary(bus => bus.Name);
        }

        public TBus Resolve<TBus>(string name) where TBus : IBus
        {
            return (TBus)Resolve(name);
        }

        private IBus Resolve(string name)
        {
            return _busDictionary.TryGetValue(name, out var bus)
                ? bus
                : throw new InvalidOperationException($"No available model for name: {name}");
        }
    }

    public class BusContextProvider : IBusContextProvider
    {
        private readonly Func<Type, IBusContext> _busContextFactory;

        public BusContextProvider(Func<Type, IBusContext> busContextFactory)
        {
            _busContextFactory = busContextFactory;
        }

        public TContext Resolve<TContext>() where TContext : IBusContext
        {
            return (TContext)_busContextFactory(typeof(TContext));
        }
    }
    public abstract class CoreBusContext<TBus> : IBusContext where TBus : IBus
    {
        private readonly IDictionary<string, TBus> _busDictionary;

        protected CoreBusContext(IEnumerable<TBus> buses)
        {
            _busDictionary = buses.ToDictionary(bus => bus.Name);
        }

        public TBus Resolve(string name)
        {
            return _busDictionary.TryGetValue(name, out var bus)
                ? bus
                : throw new InvalidOperationException($"No available bus for name: {name}");
        }
    }
    public interface IBusContext
    {
    }

    public interface IBusContextProvider
    {
    }
}
