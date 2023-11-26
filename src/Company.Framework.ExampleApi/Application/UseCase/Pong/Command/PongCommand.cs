using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command;

public record PongCommand(Guid Id, string By) : IRequest;