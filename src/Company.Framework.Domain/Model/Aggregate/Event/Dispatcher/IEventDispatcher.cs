using Company.Framework.Messaging;
using MediatR;

namespace Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;

public interface IEventDispatcher<TEvent> : IEventDispatcher, INotificationHandler<TEvent> where TEvent : IEvent
{
    Task DispatchAsync(Envelope<TEvent> envelope, CancellationToken cancellationToken);
}

public interface IEventDispatcher { }