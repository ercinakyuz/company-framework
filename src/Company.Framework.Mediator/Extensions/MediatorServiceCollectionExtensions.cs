using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Mediator.Extensions;

public static class MediatorServiceCollectionExtensions
{
    public static MediatorServiceBuilder AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            configuration.AutoRegisterRequestProcessors = true;
        });
        return new MediatorServiceBuilder(services);
    }
}