using Company.Framework.Core.Logging;

namespace Company.Framework.Messaging.Envelope
{
    public record Envelope<TMessage>(EnvelopeId Id, TMessage Message, Log Created, Correlation.CorrelationId? CorrelationId = null)
    {
        public static Envelope<TMessage> Create(TMessage message, string createdBy, Correlation.CorrelationId? correlationId = null)
        {
            return new Envelope<TMessage>(EnvelopeId.New(), message, Log.Load(createdBy), correlationId);
        }
    }
}
