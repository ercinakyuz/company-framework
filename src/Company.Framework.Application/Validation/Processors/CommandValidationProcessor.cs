using FluentValidation;
using MediatR.Pipeline;
using static Company.Framework.Core.Exception.ExceptionState;
using ApplicationException = Company.Framework.Application.Exception.ApplicationException;

namespace Company.Framework.Application.Validation.Processors
{
    public class CommandValidationProcessor<TCommand> : IRequestPreProcessor<TCommand> where TCommand : class
    {
        private readonly IValidator<TCommand> _validator;

        public CommandValidationProcessor(IValidator<TCommand> validator)
        {
            _validator = validator;
        }

        public async Task Process(TCommand command, CancellationToken cancellationToken)
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
