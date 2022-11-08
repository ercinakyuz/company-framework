using Company.Framework.Api.Localization;
using Company.Framework.Api.Localization.Context;
using Company.Framework.Api.Localization.Context.Provider;
using Company.Framework.Core.Exception;

namespace Company.Framework.Api.Models.Error.Contract.Builder
{
    public class ErrorContractBuilder
    {
        private readonly ILocalizationContext _localizationContext;

        public ErrorContractBuilder(ILocalizationContextProvider localizationContextProvider)
        {
            _localizationContext = localizationContextProvider.Resolve(LocalizationType.Internal);
        }

        public ErrorContract Build(StatefulCoreException statefulCoreException)
        {
            var (code, _, userMessage) = statefulCoreException.ActualError;
            return new ErrorContract
            {
                Code = code,
                Message = string.IsNullOrWhiteSpace(userMessage) ? _localizationContext.GetMessage(code) : userMessage
            };
        }

        public ErrorContract Build()
        {
            return new ErrorContract
            {
                Message = _localizationContext.GetMessage()
            };
        }
    }
}
