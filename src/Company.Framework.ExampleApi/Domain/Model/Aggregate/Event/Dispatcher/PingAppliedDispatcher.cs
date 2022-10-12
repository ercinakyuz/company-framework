using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.Messaging.Bus.Provider;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Kafka.Bus;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.Kafka.Producer.Args;
using Company.Framework.Messaging.RabbitMq.Producer;
using CorrelationId.Abstractions;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    private readonly IKafkaProducer<ActionId, Envelope<PingApplied>> _pingAppliedKafkaProducer1;

    private readonly IKafkaProducer<ActionId, Envelope<PingApplied>> _pingAppliedKafkaProducer2;

    private readonly IKafkaProducer _actionKafkaProducer1;

    private readonly IKafkaProducer _actionKafkaProducer2;

    private readonly IRabbitProducer _actionRabbitProducer;

    public PingAppliedDispatcher(IBusProvider busProvider,
        ICorrelationContextAccessor correlationContextAccessor,
        ILogger<PingAppliedDispatcher> logger)
        : base(correlationContextAccessor, logger)
    {
        var actionKafka1Bus = busProvider.Resolve<IKafkaBus>("ActionKafka-1");
        var actionKafka2Bus = busProvider.Resolve<IKafkaBus>("ActionKafka-2");
        _pingAppliedKafkaProducer1 = actionKafka1Bus.TypedProducerContext.Resolve<ActionId, Envelope<PingApplied>>();
        _pingAppliedKafkaProducer2 = busProvider.Resolve<IKafkaBus>("ActionKafka-2").TypedProducerContext.Resolve<ActionId, Envelope<PingApplied>>();
        _actionKafkaProducer1 = actionKafka1Bus.ProducerContext.Default();
        _actionKafkaProducer2 = actionKafka2Bus.ProducerContext.Default();
        //_actionRabbitProducer = ProducerContextProvider.Resolve<IRabbitProducerContext>().Resolve("ActionRabbit-1");
    }

    public override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        var typedKafkaProduceArgs = new KafkaProduceArgs<ActionId, Envelope<PingApplied>>(envelope.Message.AggregateId, envelope);
        var kafkaProducerArgs = new KafkaProduceArgs("ping-applied", envelope);
        await Task.WhenAll(
            _pingAppliedKafkaProducer1.ProduceAsync(typedKafkaProduceArgs, cancellationToken),
            _pingAppliedKafkaProducer2.ProduceAsync(typedKafkaProduceArgs, cancellationToken),
            _actionKafkaProducer1.ProduceAsync(kafkaProducerArgs, cancellationToken),
            _actionKafkaProducer2.ProduceAsync(kafkaProducerArgs, cancellationToken)
        );
    }
}