using Company.Framework.Core.Logging;

namespace Company.Framework.Messaging
{
    public record Envelope<TMessage>(EnvelopeCoreId CoreId, TMessage Message, Log Created)
    {
        public Correlation.CorrelationCoreId? CorrelationId { get; private set; }

        public static Envelope<TMessage> Create(TMessage message, string createdBy)
        {
            return new Envelope<TMessage>(EnvelopeCoreId.New(), message, Log.Load(createdBy));
        }

        public Envelope<TMessage> WithCorrelationId(Correlation.CorrelationCoreId correlationCoreId)
        {
            CorrelationId = correlationCoreId;
            return this;
        }
    }
}
