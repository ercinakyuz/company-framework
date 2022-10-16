using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.AdminClient.Context;

public record KafkaAdminClientContext(string BusName, IAdminClient AdminClient) : IKafkaAdminClientContext
{
    public TClient Resolve<TClient>()
    {
        if (typeof(TClient) != typeof(IAdminClient))
            throw new InvalidOperationException("Client type is not valid");
        return (TClient)AdminClient;
    }
}