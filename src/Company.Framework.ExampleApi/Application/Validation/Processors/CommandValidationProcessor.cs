using Company.Framework.ExampleApi.Application.UseCase.Ping.Command;
using FluentValidation;
using MediatR.Pipeline;
using static Company.Framework.Core.Exception.ExceptionState;
using ApplicationException = Company.Framework.Application.Exception.ApplicationException;

namespace Company.Framework.ExampleApi.Application.Validation.Processors
{
    public class PingCommandValidationProcessor : IRequestPreProcessor<PingCommand>
    {
        private readonly IValidator<PingCommand> _validator;

        public PingCommandValidationProcessor(IValidator<PingCommand> validator)
        {
            _validator = validator;
        }

        public async Task Process(PingCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken);
            if (!result.IsValid)
            {
                var error = result.Errors.First();
                throw new ApplicationException(Invalid, new(error.ErrorCode, error.ErrorMessage));
            }
        }
    }
}
