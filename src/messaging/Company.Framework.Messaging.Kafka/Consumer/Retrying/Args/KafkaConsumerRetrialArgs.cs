using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Messaging.Kafka.Model;

namespace Company.Framework.Messaging.Consumer.Retrying.Args
{
    public record KafkaConsumerRetrialArgs<TId, TMessage>(TId Id, TMessage Message, KafkaHeaders Headers, Type ExceptionType);
}
