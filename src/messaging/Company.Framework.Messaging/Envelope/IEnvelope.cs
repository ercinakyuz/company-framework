using Company.Framework.Core.Tenancy.Models;
using MediatR;

namespace Company.Framework.Messaging.Envelope
{
    public interface IEnvelope : INotification
    {
        public Correlation.CorrelationId? CorrelationId { get; }

        public TenantId? TenantId { get; }
    }
}