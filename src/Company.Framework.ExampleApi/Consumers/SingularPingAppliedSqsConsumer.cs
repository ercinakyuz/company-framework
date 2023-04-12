using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Consumers.Messages;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers;

public class SingularPingAppliedSqsConsumer : INotificationHandler<PingAppliedSqsEnvelope>
{
    private readonly ISender _sender;
    private readonly ILogger _logger;

    public SingularPingAppliedSqsConsumer(ISender sender, ILogger<PingAppliedSqsEnvelope> logger)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Handle(PingAppliedSqsEnvelope notification, CancellationToken cancellationToken)
    {
        await _sender.Send(new PongCommand(notification.Message.AggregateId), cancellationToken);
        _logger.LogInformation("Singular PingApplied RabbitEvent consumed, {notification}", notification);
    }
}