using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Company.Framework.ExampleApi.Application.UseCase.Notify.Handler;

public class NotifyCommandHandler : IRequestHandler<NotifyCommand>
{
    private readonly IHubContext<MyHub> _hubContext;

    public NotifyCommandHandler(IHubContext<MyHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(NotifyCommand command, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("notificationReceived", command.Message, cancellationToken);
    }
}