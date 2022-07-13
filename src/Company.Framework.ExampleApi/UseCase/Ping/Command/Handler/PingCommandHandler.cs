using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using MediatR;
using Action = Company.Framework.ExampleApi.Domain.Model.Aggregate.Action;

namespace Company.Framework.ExampleApi.UseCase.Ping.Command.Handler;

public class PingCommandHandler : AsyncRequestHandler<PingCommand>
{
    private readonly IActionOfWork _actionOfWork;

    public PingCommandHandler(IActionOfWork actionOfWork)
    {
        _actionOfWork = actionOfWork;
    }

    protected override async Task Handle(PingCommand request, CancellationToken cancellationToken)
    {
        await _actionOfWork.InsertAsync(Action.Create(new CreateActionDto(Log.Load("Creator"))).Ping(), cancellationToken);
    }
}