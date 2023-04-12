using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.Sqs.Consumer.Settings;

public class SqsRetrySettings : CoreRetrySettings
{
    public required SqsConsumerSettings Consumer { get; set; }

}