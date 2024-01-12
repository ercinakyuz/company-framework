using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Consumers.Messages;
using Company.Framework.Messaging.Envelope.Consumer;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers;

public class SingularPingAppliedSqsConsumer : CoreEnvelopeConsumer<PingAppliedSqsEnvelope>
{
    private readonly ISender _sender;

    public SingularPingAppliedSqsConsumer(ISender sender, IApplicationContextBuilder applicationContextBuilder, ILogger<SingularPingAppliedSqsConsumer> logger)
        : base(applicationContextBuilder, logger)
    {
        _sender = sender;
    }

    public override async Task Consume(PingAppliedSqsEnvelope notification, CancellationToken cancellationToken)
    {
        await _sender.Send(new PongCommand(notification.Message.AggregateId.Value, notification.Created.By), cancellationToken);
    }
}