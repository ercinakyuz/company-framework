using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using MediatR;

namespace Company.Framework.ExampleApi.UseCase.Pong.Command;

public record PongCommand(ActionId Id) : IRequest;