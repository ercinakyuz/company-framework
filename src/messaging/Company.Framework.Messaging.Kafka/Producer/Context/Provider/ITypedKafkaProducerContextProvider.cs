namespace Company.Framework.Messaging.Kafka.Producer.Context.Provider;

public interface ITypedKafkaProducerContextProvider
{
    ITypedKafkaProducerContext? Resolve(string busName);
}