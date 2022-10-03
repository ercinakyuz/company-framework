using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
using Company.Framework.Messaging.Envelope;
using MediatR;

namespace Company.Framework.ExampleApi.Consumers.Messages;

public record PingAppliedRabbitEnvelope(EnvelopeId Id, PingApplied Message, Log Created, Correlation.CorrelationId? CorrelationId = null)
    : Envelope<PingApplied>(Id, Message, Created, CorrelationId), INotification
{ }