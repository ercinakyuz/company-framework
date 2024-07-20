using Company.Framework.Core.Exception;
using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using Company.Framework.ExampleApi.Domain.Model.Dto;
using MediatR;
using ApplicationException = Company.Framework.Application.Exception.ApplicationException;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command.Handler;

public class PongCommandHandler : IRequestHandler<PongCommand>
{
    private readonly IActionBuilder _actionBuilder;
    private readonly IActionOfWork _actionOfWork;

    public PongCommandHandler(IActionBuilder actionBuilder, IActionOfWork actionOfWork)
    {
        _actionBuilder = actionBuilder;
        _actionOfWork = actionOfWork;
    }

    public async Task Handle(PongCommand command, CancellationToken cancellationToken)
    {
        var action = (await _actionBuilder.BuildAsync(ActionId.From(command.Id), cancellationToken))
            .ThrowOnFail(error => new ApplicationException(ExceptionState.UnProcessable, error))
            .Pong(new PongActionDto(Log.Load(command.By)));
        await _actionOfWork.UpdateAsync(action, cancellationToken);
    }
}