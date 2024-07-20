using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Query;

public record GetActionsQuery : IRequest<IEnumerable<Domain.Model.Aggregate.Action>>;
