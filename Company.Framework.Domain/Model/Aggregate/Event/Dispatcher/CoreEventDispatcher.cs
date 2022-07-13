using Company.Framework.Core.Messaging;
using MediatR;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public abstract class CoreEventDispatcher<TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
{
    public async Task Handle(TEvent @event, CancellationToken cancellationToken)
    {
        await DispatchAsync(Envelope<TEvent>.Create(@event, $"{GetType().DeclaringType}"), cancellationToken);
    }
    
    protected abstract Task DispatchAsync(Envelope<TEvent> envelope, CancellationToken cancellationToken);

}