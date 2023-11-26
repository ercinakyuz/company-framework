using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Consumers.Messages;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers;

public class SingularPingAppliedKafkaConsumer : INotificationHandler<PingAppliedKafkaEnvelope>
{
    private readonly ISender _sender;
    private readonly ILogger _logger;

    public SingularPingAppliedKafkaConsumer(ISender sender, ILogger<SingularPingAppliedKafkaConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Handle(PingAppliedKafkaEnvelope notification, CancellationToken cancellationToken)
    {
        await _sender.Send(new PongCommand(notification.Message.AggregateId.Value, notification.Created.By), cancellationToken);
        _logger.LogInformation("Singular PingApplied KafkaEvent consumed, {notification}", notification);
    }
}