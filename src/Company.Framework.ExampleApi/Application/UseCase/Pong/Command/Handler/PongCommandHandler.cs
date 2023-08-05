using Company.Framework.Core.Exception;
using Company.Framework.ExampleApi.Domain.Model.Aggregate;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using MediatR;
using ApplicationException = Company.Framework.Application.Exception.ApplicationException;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command.Handler;

public class PongCommandHandler : AsyncRequestHandler<PongCommand>
{
    private readonly IActionBuilder _actionBuilder;
    private readonly IActionOfWork _actionOfWork;

    public PongCommandHandler(IActionBuilder actionBuilder, IActionOfWork actionOfWork)
    {
        _actionBuilder = actionBuilder;
        _actionOfWork = actionOfWork;
    }

    protected override async Task Handle(PongCommand command, CancellationToken cancellationToken)
    {
        var action = (await _actionBuilder.BuildAsync(command.Id, cancellationToken))
            .ThrowOnFail(error => new ApplicationException(ExceptionState.UnProcessable, error))
            .Pong(new PongActionDto(command.Modified));
        await _actionOfWork.UpdateAsync(action, cancellationToken);
    }
}