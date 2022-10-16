namespace Company.Framework.Messaging.Kafka.AdminClient.Context.Provider;

public interface IKafkaAdminClientContextProvider
{
    IKafkaAdminClientContext Resolve(string busName);
}