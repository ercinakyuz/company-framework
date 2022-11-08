using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using Company.Framework.ExampleApi.Consumers.Messages;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers;

public class SingularPingAppliedRabbitConsumer : INotificationHandler<PingAppliedRabbitEnvelope>
{
    private readonly ISender _sender;
    private readonly ILogger _logger;

    public SingularPingAppliedRabbitConsumer(ISender sender, ILogger<SingularPingAppliedRabbitConsumer> logger)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Handle(PingAppliedRabbitEnvelope notification, CancellationToken cancellationToken)
    {
        await _sender.Send(new PongCommand(notification.Message.AggregateId), cancellationToken);
        _logger.LogInformation("Singular PingApplied RabbitEvent consumed, {}", notification);
    }
}