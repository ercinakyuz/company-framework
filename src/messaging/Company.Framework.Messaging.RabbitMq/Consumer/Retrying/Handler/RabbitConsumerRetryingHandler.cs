using System.Collections.Concurrent;
using Company.Framework.Core.Delay.Strategy;
using Company.Framework.Core.Delay.Strategy.Args;
using Company.Framework.Core.Delay.Strategy.Provider;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Consumer.Retrying.Args;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Company.Framework.Messaging.RabbitMq.Producer;
using Company.Framework.Messaging.RabbitMq.Producer.Args;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;

public class RabbitConsumerRetryingHandler : IRabbitConsumerRetryingHandler
{
    private const string RetryAttemptsHeaderKey = "retry-attempts";
    private readonly IRabbitProducer _producer;
    private readonly ConsumerRetriability _retriability;
    private readonly RabbitRetrySettings _settings;
    private readonly IDelayStrategy _delayStrategy;
    private readonly ILogger _logger;

    public RabbitDeclarationArgs DeclarationArgs => _settings.Declaration;

    public RabbitConsumerRetryingHandler(
        IRabbitProducer producer,
        ConsumerRetriability retriability,
        RabbitRetrySettings settings,
        ILogger<RabbitConsumerRetryingHandler> logger)
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
            var currentHeaders = args.Attributes as IDictionary<string, object> ?? new ConcurrentDictionary<string, object>();

            var retryAttempts = currentHeaders.TryGetValue(RetryAttemptsHeaderKey, out var retryAttemptsHeaderValue)
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
                await _producer.ProduceAsync(new RabbitProduceArgs(DeclarationArgs.Exchange, DeclarationArgs.Routing, args.Message, nextHeaders), cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Message sent to the retry exchange: {} with {} routingKey for number of {} tries: {}",
                    DeclarationArgs.Exchange, DeclarationArgs.Routing, retryAttempts, args.Message);
            }
        }
    }
}