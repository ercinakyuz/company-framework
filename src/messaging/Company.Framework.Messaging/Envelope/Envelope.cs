using Company.Framework.Core.Logging;
using Company.Framework.Core.Tenancy.Models;
using MediatR;

namespace Company.Framework.Messaging.Envelope
{
    public record Envelope<TMessage>(EnvelopeId Id, TMessage Message, Log Created, TenantId? TenantId = default, Correlation.CorrelationId? CorrelationId = default) : IEnvelope
    {
        public static Envelope<TMessage> Create(TMessage message, string createdBy, TenantId? tenantId = default, Correlation.CorrelationId? correlationId = default)
        {
            return new Envelope<TMessage>(EnvelopeId.New(), message, Log.Load(createdBy), tenantId, correlationId);
        }
    }
}
