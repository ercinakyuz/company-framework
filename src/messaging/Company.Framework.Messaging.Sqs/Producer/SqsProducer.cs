using Amazon.SQS;
using Amazon.SQS.Model;
using Company.Framework.Core.Serialization;
using Company.Framework.Messaging.Sqs.Client.Context;
using Company.Framework.Messaging.Sqs.Producer.Args;

namespace Company.Framework.Messaging.Sqs.Producer
{
    public class SqsProducer : ISqsProducer
    {
        public string BusName { get; }
        public string Name { get; }

        private readonly IAmazonSQS _client;

        private readonly IJsonSerializer _jsonSerializer;

        public SqsProducer(string name, string busName, ISqsClientContext clientContext, IJsonSerializer jsonSerializer)
        {
            Name = name;
            BusName = busName;
            _jsonSerializer = jsonSerializer;
            _client = clientContext.Resolve<IAmazonSQS>();
        }

        public async Task ProduceAsync(SqsProduceArgs args, CancellationToken cancellationToken)
        {
            var (queue, message, attributes) = args;

            var getQueueUrlResponse = await _client.GetQueueUrlAsync(queue, cancellationToken).ConfigureAwait(false);

            var request = new SendMessageRequest
            {
                QueueUrl = getQueueUrlResponse.QueueUrl,
                MessageBody = _jsonSerializer.Serialize(message),
            };

            if (attributes != default)
            {
                var typedAttributes = new Dictionary<string, MessageAttributeValue>();
                foreach (var attribute in attributes)
                {
                    typedAttributes.Add(attribute.Key, (MessageAttributeValue)attribute.Value);
                }
                request.MessageAttributes = typedAttributes;
            }

            await _client.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);

        }
    }
}
