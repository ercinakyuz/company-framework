using Microsoft.AspNetCore.Builder;

namespace Company.Framework.Api.Localization.Extensions;

public static class LocalizationApplicationBuilderExtensions
{
    public static IApplicationBuilder UseLocalization(this IApplicationBuilder applicationBuilder)
    {
        var supportedCultures = new[] { "tr-TR", "en-US" };
        return applicationBuilder.UseRequestLocalization(new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures));
    }
}