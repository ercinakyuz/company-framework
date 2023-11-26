using Company.Framework.ExampleApi.Application.UseCase.Pong.Command;
using FluentValidation;

namespace Company.Framework.ExampleApi.Application.UseCase.Ping.Command.Validator
{
    public class PingCommandValidator : AbstractValidator<PongCommand>
    {
        public PingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.By).NotEmpty();
        }
    }
}
