using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.Ping.Command;

public record PingCommand : IRequest<Guid>;