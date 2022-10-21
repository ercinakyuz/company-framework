namespace Company.Framework.Api.Localization.Context.Provider
{
    public interface ILocalizationContextProvider
    {
        ILocalizationContext Resolve(LocalizationType type);
    }
}
