using MediatR;

namespace Company.Framework.ExampleApi.UseCase.Ping.Command;

public record PingCommand : IRequest<Guid>;