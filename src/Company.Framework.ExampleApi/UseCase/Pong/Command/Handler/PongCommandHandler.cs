using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using Company.Framework.ExampleApi.Http.Clients;
using MediatR;

namespace Company.Framework.ExampleApi.UseCase.Pong.Command.Handler;

public class PongCommandHandler : AsyncRequestHandler<PongCommand>
{
    private readonly IActionBuilder _actionBuilder;
    private readonly IActionOfWork _actionOfWork;
    private readonly IActionHttpClient _actionHttpClient;

    public PongCommandHandler(IActionBuilder actionBuilder, IActionOfWork actionOfWork, IActionHttpClient actionHttpClient)
    {
        _actionBuilder = actionBuilder;
        _actionOfWork = actionOfWork;
        _actionHttpClient = actionHttpClient;
    }

    protected override async Task Handle(PongCommand request, CancellationToken cancellationToken)
    {
        var action = await _actionBuilder.BuildAsync(request.Id, cancellationToken);
        action.Pong();
        await _actionOfWork.UpdateAsync(action, cancellationToken);
        //await _actionHttpClient.PingAsync(cancellationToken);
    }
}