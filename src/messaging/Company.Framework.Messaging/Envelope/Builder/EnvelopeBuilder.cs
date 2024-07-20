using Company.Framework.Core.Tenancy.Components;
using CorrelationId.Abstractions;

namespace Company.Framework.Messaging.Envelope.Builder
{
    public class EnvelopeBuilder
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        private readonly ITenantAccessor _tenantAccessor;

        public EnvelopeBuilder(ICorrelationContextAccessor correlationContextAccessor, ITenantAccessor tenantAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
            _tenantAccessor = tenantAccessor;
        }

        public Envelope<TMessage> Build<TMessage>(TMessage message, string by)
        {
            var tenantId = _tenantAccessor.Get().Data?.Id;
            var correlationId = Correlation.CorrelationId.From(_correlationContextAccessor.CorrelationContext.CorrelationId);
            return Envelope<TMessage>.Create(message, by, tenantId, correlationId);
        }
    }
}
