using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.Sqs.Producer.Context
{
    public class SqsProducerContext : CoreProducerContext<ISqsProducer>, ISqsProducerContext
    {
        public SqsProducerContext(IEnumerable<ISqsProducer> producers) : base(producers)
        {
        }
    }
}
