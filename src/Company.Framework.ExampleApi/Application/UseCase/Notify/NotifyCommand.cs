using MediatR;

namespace Company.Framework.ExampleApi.Application.UseCase.Notify;

public record NotifyCommand(string Message) : IRequest;