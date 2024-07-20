using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.GetAllActions.Stream
{
    public record GetActionStream : IStreamRequest<Domain.Model.Aggregate.Action>;
}
