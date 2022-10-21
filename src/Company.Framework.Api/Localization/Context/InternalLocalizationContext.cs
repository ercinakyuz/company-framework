using Company.Framework.Core.Error;
using Microsoft.Extensions.Localization;

namespace Company.Framework.Api.Localization.Context;

public class InternalLocalizationContext<TResource> : ILocalizationContext
{
    public LocalizationType Type => LocalizationType.Internal;

    private readonly IStringLocalizer _stringLocalizer;

    public InternalLocalizationContext(IStringLocalizer<TResource> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    public string GetMessage(string? key = default)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            key = GenericError.Code;
        }

        var localizedString = _stringLocalizer[key];

        return localizedString.ResourceNotFound ? GenericError.Message : localizedString.Value;
    }
}