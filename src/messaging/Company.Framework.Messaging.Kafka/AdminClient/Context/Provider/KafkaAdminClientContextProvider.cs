namespace Company.Framework.Messaging.Kafka.AdminClient.Context.Provider;

public class KafkaAdminClientContextProvider : IKafkaAdminClientContextProvider
{

    private readonly IReadOnlyDictionary<string, IKafkaAdminClientContext> _contextsByBus;


    public KafkaAdminClientContextProvider(IEnumerable<IKafkaAdminClientContext> contexts)
    {
        _contextsByBus = contexts.ToDictionary(context => context.BusName);
    }

    public IKafkaAdminClientContext Resolve(string busName)
    {
        if (!_contextsByBus.TryGetValue(busName, out var context))
            throw new InvalidOperationException(
                $"No available kafka admin client contexts for given bus name of '{busName}'");
        return context;
    }
}