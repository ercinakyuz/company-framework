using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Sqs.Client.Context;
using Company.Framework.Messaging.Sqs.Consumer.Settings;

namespace Company.Framework.Messaging.Sqs.Consumer.Context
{
    public class SqsConsumerContext : ISqsConsumerContext
    {
        public ISqsClientContext ClientContext { get; }
        public SqsConsumerSettings Settings { get; }

        public IJsonSerializer JsonSerializer { get; }

        public SqsConsumerContext(ISqsClientContext clientContext, SqsConsumerSettings settings, IJsonSerializer jsonSerializer)
        {
            ClientContext = clientContext;
            Settings = settings;
            JsonSerializer = jsonSerializer;
        }
    }
}
