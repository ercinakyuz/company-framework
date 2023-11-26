using Company.Framework.Application.Validation.Processors;
using Company.Framework.Mediator.Extensions;

namespace Company.Framework.Application.Validation.Extensions;

public static class MediatorServiceBuilderExtensions
{
    public static MediatorServiceBuilder WithCommandValidation(this MediatorServiceBuilder builder)
    {
        return builder.WithPreProcessor(typeof(CommandValidationProcessor<>));
    }
}