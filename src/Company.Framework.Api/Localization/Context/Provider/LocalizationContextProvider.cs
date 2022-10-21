namespace Company.Framework.Api.Localization.Context.Provider;

public class LocalizationContextProvider : ILocalizationContextProvider
{
    private readonly IReadOnlyDictionary<LocalizationType, ILocalizationContext> _localizerContextMap;


    public LocalizationContextProvider(IEnumerable<ILocalizationContext> contexts)
    {
        _localizerContextMap = contexts.ToDictionary(context => context.Type);
    }

    public ILocalizationContext Resolve(LocalizationType type)
    {
        if (!_localizerContextMap.TryGetValue(type, out var context))
            throw new InvalidOperationException(
                $"No available localizer context for given type");
        return context;
    }
}