using Company.Framework.Domain.Model.Aggregate.Event.Dispatcher;
using Company.Framework.Infrastructure.Application.Context.Builder.Args;
using Microsoft.Extensions.Logging;

namespace Company.Framework.Messaging.Envelope.Consumer;

public abstract class CoreEnvelopeConsumer<TEnvelope> : IEnvelopeConsumer<TEnvelope> where TEnvelope : IEnvelope
{
    private readonly IApplicationContextBuilder _applicationContextBuilder;
    private readonly ILogger _logger;


    protected CoreEnvelopeConsumer(IApplicationContextBuilder applicationContextBuilder, ILogger logger)
    {
        _applicationContextBuilder = applicationContextBuilder;
        _logger = logger;
    }

    public abstract Task Consume(TEnvelope envelope, CancellationToken cancellationToken);

    public async Task Handle(TEnvelope envelope, CancellationToken cancellationToken)
    {
        _logger.LogInformation("{envelope} consuming, {notification}", nameof(envelope), envelope);
        await ConsumeInternal(envelope, cancellationToken);
        _logger.LogInformation("{envelope} consumed, {notification}", nameof(envelope), envelope);
    }

    private async Task ConsumeInternal(TEnvelope envelope, CancellationToken cancellationToken)
    {
        _applicationContextBuilder.Build(new ApplicationContextBuilderArgs(envelope.CorrelationId, envelope.TenantId));
        await Consume(envelope, cancellationToken);
    }

}