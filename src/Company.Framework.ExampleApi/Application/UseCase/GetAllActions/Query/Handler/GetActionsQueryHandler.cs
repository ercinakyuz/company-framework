using Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;
using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Query.Handler
{
    public class GetActionsQueryHandler(IActionBuilder actionBuilder) : IRequestHandler<GetActionsQuery, IEnumerable<Domain.Model.Aggregate.Action>>
    {
        public async Task<IEnumerable<Domain.Model.Aggregate.Action>> Handle(GetActionsQuery request, CancellationToken cancellationToken)
        {
            return await actionBuilder.BuildAllAsync(cancellationToken);
        }
    }
}
