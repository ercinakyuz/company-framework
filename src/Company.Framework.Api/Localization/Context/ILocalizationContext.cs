using Company.Framework.Api.Localization.Context.Provider;

namespace Company.Framework.Api.Localization.Context
{
    public interface ILocalizationContext
    {
        LocalizationType Type { get; }

        string GetMessage(string? key = default);
    }
}
