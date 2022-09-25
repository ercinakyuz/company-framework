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
}
