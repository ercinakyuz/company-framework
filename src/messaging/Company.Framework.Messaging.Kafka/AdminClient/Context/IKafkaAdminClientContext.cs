namespace Company.Framework.Messaging.Kafka.AdminClient.Context;

public interface IKafkaAdminClientContext
{
    string BusName { get; }
    TClient Resolve<TClient>();
}