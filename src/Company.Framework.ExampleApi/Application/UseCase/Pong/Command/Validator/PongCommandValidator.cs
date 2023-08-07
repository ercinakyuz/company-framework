using FluentValidation;

namespace Company.Framework.ExampleApi.Application.UseCase.Pong.Command.Validator
{
    public class PongCommandValidator : AbstractValidator<PongCommand>
    {
        public PongCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
