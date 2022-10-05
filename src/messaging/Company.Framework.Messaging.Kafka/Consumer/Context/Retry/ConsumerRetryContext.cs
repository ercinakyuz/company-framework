using System.Text.Json;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Kafka.Consumer.Context.Retry
{
    public class KafkaRetrialContext<TMessage> : IKafkaRetrialContext
    {
        private const string RetryAttemptsHeaderKey = "retry-attempts";
        private readonly IProducer<Null, TMessage> _producer;
        private readonly ConsumerRetriability _retriability;
        private readonly KafkaRetrySettings _settings;
        private readonly ILogger _logger;

        public string Topic => _settings.Topic;

        public KafkaRetrialContext(IProducer<Null, TMessage> producer, ConsumerRetriability retriability, KafkaRetrySettings settings, ILogger<KafkaRetrialContext<TMessage>> logger)
        {
            _producer = producer;
            _retriability = retriability;
            _logger = logger;
            _settings = settings;
        }
        
        public async Task RetryAsync(object message, Type exceptionType, CancellationToken cancellationToken)
        {
            if (message is not Message<Null, TMessage> retryMessage)
                throw new ArgumentException("message is not a valid type");
            if (_retriability.IsRetriableException(exceptionType))
            {
                var retryAttempts = retryMessage.Headers.TryGetLastBytes(RetryAttemptsHeaderKey, out var lastHeader)
                    ? JsonSerializer.Deserialize<short>(lastHeader)
                    : (short)0;
                if (_settings.Count > retryAttempts)
                {
                    retryAttempts++;
                    await Task.Delay(TimeSpan.FromMilliseconds(_settings.ExponentialIntervalMs * retryAttempts), cancellationToken);
                    retryMessage.Headers.Add(RetryAttemptsHeaderKey, JsonSerializer.SerializeToUtf8Bytes(retryAttempts));
                    await _producer.ProduceAsync(_settings.Topic, retryMessage, cancellationToken);
                    _logger.LogInformation("Message sent to the retry topic: {} for number of {} tries: {}", _settings.Topic,
                        retryAttempts, retryMessage.Value);
                }
            }
        }
    }
}