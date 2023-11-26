using Company.Framework.ExampleApi.Application.UseCase.Ping.Command;
using FluentValidation;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command.Validator
{
    public class PongCommandValidator : AbstractValidator<PongCommand>
    {
        public PongCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.By).NotEmpty();
        }
    }
}
