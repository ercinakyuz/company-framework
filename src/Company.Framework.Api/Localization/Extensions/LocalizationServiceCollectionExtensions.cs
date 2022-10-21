using Company.Framework.Api.Localization.Context;
using Company.Framework.Api.Localization.Context.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Api.Localization.Extensions
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalization<TResource>(this IServiceCollection services)
        {
            return
                services
                    .AddSingleton<ILocalizationContextProvider, LocalizationContextProvider>()
                    .AddSingleton<ILocalizationContext, InternalLocalizationContext<TResource>>()
                    .AddLocalization();
        }
    }
}
