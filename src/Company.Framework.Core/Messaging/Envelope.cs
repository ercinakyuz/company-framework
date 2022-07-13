using Company.Framework.Core.Logging;

namespace Company.Framework.Core.Messaging
{
    public record Envelope<TMessage>(Guid Id, TMessage Message, Log Created, Guid? CorrelationId = null)
    {
        public static Envelope<TMessage> Create(TMessage message, string createdBy)
        {
            return new Envelope<TMessage>(Guid.NewGuid(), message, Log.Load(createdBy));
        }
    }
}
