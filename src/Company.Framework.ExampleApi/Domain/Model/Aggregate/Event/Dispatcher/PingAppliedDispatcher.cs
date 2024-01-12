using Company.Framework.Core.Tenancy.Models;
using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.Messaging.Bus.Provider;
using Company.Framework.Messaging.Envelope;
using Company.Framework.Messaging.Envelope.Builder;
using Company.Framework.Messaging.Kafka.Producer;
using Company.Framework.Messaging.RabbitMq.Producer;
using Company.Framework.Messaging.Sqs.Bus;
using Company.Framework.Messaging.Sqs.Producer;
using Company.Framework.Messaging.Sqs.Producer.Args;
using CorrelationId.Abstractions;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Event.Dispatcher;

public class PingAppliedDispatcher : CoreEventDispatcher<PingApplied>
{
    private readonly IKafkaProducer<ActionId, Envelope<PingApplied>> _pingAppliedKafkaProducer1;

    private readonly IKafkaProducer<ActionId, Envelope<PingApplied>> _pingAppliedKafkaProducer2;

    private readonly IKafkaProducer _actionKafkaProducer1;

    private readonly IKafkaProducer _actionKafkaProducer2;

    private readonly IRabbitProducer _actionRabbitProducer1;

    private readonly IRabbitProducer _actionRabbitProducer2;

    private readonly ISqsProducer _actionSqsProducer1;

    public PingAppliedDispatcher(IBusProvider busProvider,
        EnvelopeBuilder envelopeBuilder,
        ILogger<PingAppliedDispatcher> logger)
        : base(envelopeBuilder, logger)
    {
        //var actionKafka1Bus = busProvider.Resolve<IKafkaBus>("ActionKafka-1");
        //var actionKafka2Bus = busProvider.Resolve<IKafkaBus>("ActionKafka-2");
        //var actionRabbit1Bus = busProvider.Resolve<IRabbitBus>("ActionRabbit-1");
        //var actionRabbit2Bus = busProvider.Resolve<IRabbitBus>("ActionRabbit-2");
        var actionSqs1Bus = busProvider.Resolve<ISqsBus>("ActionSqs-1");

        //_pingAppliedKafkaProducer1 = actionKafka1Bus.TypedProducerContext!.Resolve<ActionId, Envelope<PingApplied>>();
        //_pingAppliedKafkaProducer2 = actionKafka2Bus.TypedProducerContext!.Resolve<ActionId, Envelope<PingApplied>>();
        //_actionKafkaProducer1 = actionKafka1Bus.ProducerContext.Default();
        //_actionKafkaProducer2 = actionKafka2Bus.ProducerContext.Default();
        //_actionRabbitProducer1 = actionRabbit1Bus.ProducerContext.Default();
        //_actionRabbitProducer2 = actionRabbit2Bus.ProducerContext.Default();

        _actionSqsProducer1 = actionSqs1Bus.ProducerContext.Default();
    }

    public override async Task DispatchAsync(Envelope<PingApplied> envelope, CancellationToken cancellationToken)
    {
        //var typedKafkaProduceArgs = new KafkaProduceArgs<ActionId, Envelope<PingApplied>>(envelope.Message.AggregateId, envelope);
        //var kafkaProducerArgs = new KafkaProduceArgs("ping-applied", envelope);
        //var rabbitProducerArgs = new RabbitProduceArgs(new RabbitExchangeArgs { Name = "action", Type = "topic" }, "ping-applied", envelope);
        var sqsProducerArgs = new SqsProduceArgs("action-ping-applied", envelope);

        await Task.WhenAll(
            //_pingAppliedKafkaProducer1.ProduceAsync(typedKafkaProduceArgs, cancellationToken),
            //_pingAppliedKafkaProducer2.ProduceAsync(typedKafkaProduceArgs, cancellationToken),
            //_actionKafkaProducer1.ProduceAsync(kafkaProducerArgs, cancellationToken),
            //_actionKafkaProducer2.ProduceAsync(kafkaProducerArgs, cancellationToken),
            //_actionRabbitProducer1.ProduceAsync(rabbitProducerArgs, cancellationToken),
            //_actionRabbitProducer2.ProduceAsync(rabbitProducerArgs, cancellationToken),
            _actionSqsProducer1.ProduceAsync(sqsProducerArgs, cancellationToken)
        );
    }
}