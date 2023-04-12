using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Sqs.Producer.Context;

namespace Company.Framework.Messaging.Sqs.Bus
{
    public interface ISqsBus : IBus
    {
        ISqsProducerContext ProducerContext { get; }
    }
}
