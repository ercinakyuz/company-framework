using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command;

public record PongCommand(ActionId Id, Log Modified) : IRequest;