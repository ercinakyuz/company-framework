using Company.Framework.Messaging.Producer.Args;

namespace Company.Framework.Messaging.Sqs.Producer.Args;

public record SqsProduceArgs(string Queue, object Message, IDictionary<string, object>? Attributes = default) : CoreProduceArgs(Message);