using Company.Framework.Messaging.Envelope.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Envelope.Builder.Extensions
{
    public static class EnvelopeServiceCollectionExtensions
    {
        public static IServiceCollection AddEnvelope(this IServiceCollection services)
        {
            return services
                .AddSingleton<EnvelopeBuilder>();
        }
    }
}
