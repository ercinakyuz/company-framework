using Company.Framework.Core.Logging;
using Company.Framework.Core.Tenancy.Models;
using Company.Framework.ExampleApi.Domain.Model.Aggregate;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using MediatR;
using Action = Company.Framework.ExampleApi.Domain.Model.Aggregate.Action;

namespace Company.Framework.ExampleApi.Application.UseCase.Ping.Command.Handler;

public class PingCommandHandler : IRequestHandler<PingCommand, ActionId>
{
    private readonly IActionOfWork _actionOfWork;
    private readonly ITenantAccessor _tenantAccessor;

    public PingCommandHandler(IActionOfWork actionOfWork, ITenantAccessor tenantAccessor)
    {
        _actionOfWork = actionOfWork;
        _tenantAccessor = tenantAccessor;
    }

    public async Task<ActionId> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        var tenant = _tenantAccessor.Get().OrElseThrow(() => new Exception());
        var action = Action.Ping(new PingActionDto(Log.Load(request.By)));
        await _actionOfWork.InsertAsync(action, cancellationToken);
        return action.Id;
    }
}