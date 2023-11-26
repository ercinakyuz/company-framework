using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.Ping.Command;

public record PingCommand(string By) : IRequest<ActionId>;