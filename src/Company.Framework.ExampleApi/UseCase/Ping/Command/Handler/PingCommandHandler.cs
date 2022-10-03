using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using MediatR;
using Action = Company.Framework.ExampleApi.Domain.Model.Aggregate.Action;

namespace Company.Framework.ExampleApi.UseCase.Ping.Command.Handler;

public class PingCommandHandler : IRequestHandler<PingCommand,Guid>
{
    private readonly IActionOfWork _actionOfWork;

    public PingCommandHandler(IActionOfWork actionOfWork)
    {
        _actionOfWork = actionOfWork;
    }

    public async Task<Guid> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        var action = Action.Ping(new PingActionDto(Log.Load("Creator")));
        await _actionOfWork.InsertAsync(action, cancellationToken);
        return action.Id.Value;
    }
}