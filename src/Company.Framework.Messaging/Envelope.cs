using Company.Framework.Core.Logging;

namespace Company.Framework.Messaging
{
    public record Envelope<TMessage>(EnvelopeId Id, TMessage Message, Log Created)
    {
        public Correlation.CorrelationId? CorrelationId { get; private set; }

        public static Envelope<TMessage> Create(TMessage message, string createdBy)
        {
            return new Envelope<TMessage>(EnvelopeId.New(), message, Log.Load(createdBy));
        }

        public Envelope<TMessage> WithCorrelationId(Correlation.CorrelationId correlationId)
        {
            CorrelationId = correlationId;
            return this;
        }
    }
}
