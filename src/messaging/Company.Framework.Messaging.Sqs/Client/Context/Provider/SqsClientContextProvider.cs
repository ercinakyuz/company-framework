
namespace Company.Framework.Messaging.Sqs.Client.Context.Provider;

public class SqsClientContextProvider : ISqsClientContextProvider
{

    private readonly IReadOnlyDictionary<string, ISqsClientContext> _connectionContextsByBus;


    public SqsClientContextProvider(IEnumerable<ISqsClientContext> contexts)
    {
        _connectionContextsByBus = contexts.ToDictionary(context => context.BusName);
    }

    public ISqsClientContext Resolve(string busName)
    {
        if (!_connectionContextsByBus.TryGetValue(busName, out var context))
            throw new InvalidOperationException(
                $"No available sqs contexts for given bus name of: {busName}");
        return context;
    }
}