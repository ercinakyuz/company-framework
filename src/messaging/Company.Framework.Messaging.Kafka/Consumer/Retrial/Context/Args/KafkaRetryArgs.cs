namespace Company.Framework.Messaging.Kafka.Consumer.Retrial.Context.Args
{
    public record KafkaRetryArgs(object Message, object Headers, Type ExceptionType);
}
