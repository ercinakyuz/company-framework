using System.Collections.Concurrent;
using Company.Framework.Core.Delay.Strategy;
using Company.Framework.Core.Delay.Strategy.Args;
using Company.Framework.Core.Delay.Strategy.Provider;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.Sqs.Consumer.Settings;
using Company.Framework.Messaging.Sqs.Producer;
using Company.Framework.Messaging.Sqs.Producer.Args;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;

public class SqsConsumerRetryingHandler : ISqsConsumerRetryingHandler
{
    private const string RetryAttemptsHeaderKey = "retry-attempts";
    private readonly ISqsProducer _producer;
    private readonly ConsumerRetriability _retriability;
    private readonly SqsRetrySettings _settings;
    private readonly IDelayStrategy _delayStrategy;
    private readonly ILogger _logger;

    public string Queue => _settings.Consumer.Queue;

    public SqsConsumerRetryingHandler(
        ISqsProducer producer,
        ConsumerRetriability retriability,
        SqsRetrySettings settings,
        ILogger<SqsConsumerRetryingHandler> logger)
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
            var currentAttributes = args.Attributes as IDictionary<string, object> ?? new ConcurrentDictionary<string, object>();

            var retryAttempts = currentAttributes.TryGetValue(RetryAttemptsHeaderKey, out var retryAttemptsHeaderValue)
                ? short.Parse(retryAttemptsHeaderValue.ToString()!)
                : (short)0;
            if (_settings.Count > retryAttempts)
            {
                retryAttempts++;
                await _delayStrategy.DelayAsync(new DelayStrategyArgs(_settings.Delay.Interval, retryAttempts), cancellationToken).ConfigureAwait(false);
                var nextHeaders = new ConcurrentDictionary<string, object>
                {
                    [RetryAttemptsHeaderKey] = retryAttempts
                };
                await _producer.ProduceAsync(new SqsProduceArgs(Queue, args.Message, nextHeaders), cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Message sent to the retry queue: {} for number of {} tries: {}",
                    Queue, retryAttempts, args.Message);
            }
        }
    }
}