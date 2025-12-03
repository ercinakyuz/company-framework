using MediatR;
using NUlid;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command;

public record PongCommand(Ulid Id, string By) : IRequest;