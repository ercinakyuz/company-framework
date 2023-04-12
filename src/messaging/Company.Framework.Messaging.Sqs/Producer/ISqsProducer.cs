using Company.Framework.Messaging.Producer;
using Company.Framework.Messaging.Sqs.Producer.Args;

namespace Company.Framework.Messaging.Sqs.Producer;

public interface ISqsProducer : IProducer<SqsProduceArgs>
{
}