namespace Company.Framework.Messaging.RabbitMq.Connection.Context.Provider;

public class RabbitConnectionContextProvider : IRabbitConnectionContextProvider
{

    private readonly IReadOnlyDictionary<string, IRabbitConnectionContext> _connectionContextsByBus;


    public RabbitConnectionContextProvider(IEnumerable<IRabbitConnectionContext> contexts)
    {
        _connectionContextsByBus = contexts.ToDictionary(context => context.BusName);
    }

    public IRabbitConnectionContext Resolve(string busName)
    {
        if (!_connectionContextsByBus.TryGetValue(busName, out var context))
            throw new InvalidOperationException(
                $"No available rabbit contexts for given bus name of: {busName}");
        return context;
    }
}