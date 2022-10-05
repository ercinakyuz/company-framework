using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Args;
using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Producer.Context.Provider;
using Company.Framework.Messaging.RabbitMq.Producer;
using Company.Framework.Messaging.RabbitMq.Producer.Args;
using Company.Framework.Messaging.RabbitMq.Producer.Context;
using CorrelationId.Abstractions;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    private readonly IKafkaProducer _actionKafkaProducer;

    private readonly IRabbitProducer _actionRabbitProducer;

    public PingAppliedDispatcher(
        IProducerContextProvider producerContextProvider,
        ICorrelationContextAccessor correlationContextAccessor,
        ILogger<PingAppliedDispatcher> logger)
        : base(producerContextProvider, correlationContextAccessor, logger)
    {
        _actionKafkaProducer = ProducerContextProvider.Resolve<IKafkaProducerContext>().Resolve("ActionKafka-1");
        //_actionRabbitProducer = ProducerContextProvider.Resolve<IRabbitProducerContext>().Resolve("ActionRabbit-1");
    }

    public override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        await Task.WhenAll(
             _actionKafkaProducer.ProduceAsync(new KafkaProduceArgs("ping-applied", envelope), cancellationToken)
             //,_actionRabbitProducer.ProduceAsync(new RabbitProduceArgs(new ExchangeArgs("action", "topic"), "ping-applied", envelope), cancellationToken)
            );
    }
}