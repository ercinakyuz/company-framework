using Company.Framework.Core.Delay.Strategy;
using Company.Framework.Core.Delay.Strategy.Args;
using Company.Framework.Core.Delay.Strategy.Provider;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Args;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Kafka.Consumer.Retrying.Handler;

public class KafkaConsumerRetryingHandler : IKafkaConsumerRetryingHandler
{
    private const string RetryAttemptsHeaderKey = "retry-attempts";
    private readonly IKafkaProducer _producer;
    private readonly ConsumerRetriability _retriability;
    private readonly KafkaRetrySettings _settings;
    private readonly IDelayStrategy _delayStrategy;
    private readonly ILogger _logger;

    public KafkaTopicSettings TopicSettings => _settings.Topic;

    public KafkaConsumerRetryingHandler(IKafkaProducer producer, ConsumerRetriability retriability, KafkaRetrySettings settings, ILogger<KafkaConsumerRetryingHandler> logger)
    {
        _producer = producer;
        _retriability = retriability;
        _logger = logger;
        _settings = settings;
        _delayStrategy = DelayStrategyProvider.Resolve(settings.Delay.Type);
    }

    public async Task HandleAsync(ConsumerRetrialArgs args, CancellationToken cancellationToken)
    {
        if (_retriability.IsRetriableException(args.ExceptionType))
        {
            var currentHeaders = (Headers)args.Attributes;
            var retryAttempts = currentHeaders.TryGetLastBytes(RetryAttemptsHeaderKey, out var lastHeader)
                ? BitConverter.ToInt16(lastHeader)
                : (short)0;
            if (_settings.Count > retryAttempts)
            {
                retryAttempts++;
                await _delayStrategy.DelayAsync(new DelayStrategyArgs(_settings.Delay.Interval, retryAttempts), cancellationToken).ConfigureAwait(false);
                var nextHeaders = new Headers { { RetryAttemptsHeaderKey, BitConverter.GetBytes(retryAttempts) } };
                await _producer.ProduceAsync(new KafkaProduceArgs(TopicSettings.Name, args.Message, nextHeaders), cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Message sent to the retry topic: {topic} for number of {attempts} tries: {message}", _settings.Topic, retryAttempts, args.Message);
            }
        }
    }
}
public class KafkaConsumerRetryingHandler<TId, TMessage> : IKafkaConsumerRetryingHandler<TId, TMessage>
{
    private const string RetryAttemptsHeaderKey = "retry-attempts";
    private readonly IKafkaProducer<TId, TMessage> _producer;
    private readonly ConsumerRetriability _retriability;
    private readonly KafkaRetrySettings _settings;
    private readonly IDelayStrategy _delayStrategy;
    private readonly ILogger _logger;

    public KafkaTopicSettings TopicSettings => _settings.Topic;

    public KafkaConsumerRetryingHandler(IKafkaProducer<TId, TMessage> producer, ConsumerRetriability retriability, KafkaRetrySettings settings, ILogger<KafkaConsumerRetryingHandler> logger)
    {
        _producer = producer;
        _retriability = retriability;
        _logger = logger;
        _settings = settings;
        _delayStrategy = DelayStrategyProvider.Resolve(settings.Delay.Type);
    }

    public async Task HandleAsync(KafkaConsumerRetrialArgs<TId, TMessage> args, CancellationToken cancellationToken)
    {
        if (_retriability.IsRetriableException(args.ExceptionType))
        {
            var headers = args.Headers;
            headers.TryGetValue<short>(RetryAttemptsHeaderKey, out var retryAttempts);
            if (_settings.Count > retryAttempts)
            {
                retryAttempts++;
                await _delayStrategy.DelayAsync(new DelayStrategyArgs(_settings.Delay.Interval, retryAttempts), cancellationToken).ConfigureAwait(false);
                headers.Replace(RetryAttemptsHeaderKey, retryAttempts);
                await _producer.ProduceAsync(new KafkaProduceArgs<TId, TMessage>(args.Id, args.Message, headers, TopicSettings.Name), cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Message sent to the retry topic: {topic} for number of {attempts} tries: {message}", _settings.Topic, retryAttempts, args.Message);
            }
        }
    }
}
