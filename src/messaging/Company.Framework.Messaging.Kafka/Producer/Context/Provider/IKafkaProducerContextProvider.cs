namespace Company.Framework.Messaging.Kafka.Producer.Context.Provider;

public interface IKafkaProducerContextProvider
{
    IKafkaProducerContext Resolve(string busName);
}