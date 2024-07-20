using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Stream.Handler
{
    public class GetActionStreamHandler(IActionBuilder actionBuilder) : IStreamRequestHandler<GetActionStream, Domain.Model.Aggregate.Action>
    {
        public IAsyncEnumerable<Domain.Model.Aggregate.Action> Handle(GetActionStream request, CancellationToken cancellationToken)
        {
            return actionBuilder.BuildAllStreaming(cancellationToken);
        }
    }
}
