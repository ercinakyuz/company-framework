using Company.Framework.Api.Localization;
using Company.Framework.Api.Localization.Context;
using Company.Framework.Api.Localization.Context.Provider;
using Company.Framework.Core.Exception;

namespace Company.Framework.Api.Models.Error.Contract.Builder
{
    public class ErrorContractBuilder
    {
        private readonly ILocalizationContext _localizerContext;

        public ErrorContractBuilder(ILocalizationContextProvider localizerContextProvider)
        {
            _localizerContext = localizerContextProvider.Resolve(LocalizationType.Internal);
        }

        public ErrorContract Build(CoreException coreException)
        {
            return new ErrorContract
            {
                Code = coreException.Code,
                Message = string.IsNullOrWhiteSpace(coreException.UserMessage) ? _localizerContext.GetMessage(coreException.Code) : coreException.UserMessage
            };
        }

        public ErrorContract Build()
        {
            return new ErrorContract
            {
                Message = _localizerContext.GetMessage()
            };
        }
    }
}
