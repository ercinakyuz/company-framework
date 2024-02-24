using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.ExampleApi.Application.UseCase.Notify;
using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Consumers.Messages;
using Company.Framework.Messaging.Envelope.Consumer;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers;

public class SingularPingAppliedKafkaConsumer : CoreEnvelopeConsumer<PingAppliedKafkaEnvelope>
{
    private readonly ISender _sender;

    public SingularPingAppliedKafkaConsumer(ISender sender, IApplicationContextBuilder applicationContextBuilder, ILogger<SingularPingAppliedKafkaConsumer> logger)
        : base(applicationContextBuilder, logger)
    {
        _sender = sender;
    }

    public override async Task Consume(PingAppliedKafkaEnvelope notification, CancellationToken cancellationToken)
    {
        await _sender.Send(new NotifyCommand("Notification from Hub-1 received!"), cancellationToken);
        //await _sender.Send(new PongCommand(notification.Message.AggregateId.Value, notification.Created.By), cancellationToken);
    }
}