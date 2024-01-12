using Company.Framework.Messaging.Envelope;
using MediatR;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher
{
    public interface IEnvelopeConsumer<TEnvelope> : INotificationHandler<TEnvelope> where TEnvelope : IEnvelope
    {
    }
}