﻿//using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
//using Company.Framework.Messaging.Envelope;
//using Company.Framework.Messaging.RabbitMq.ConnectionContext.Context;
//using Company.Framework.Messaging.RabbitMq.Consumer;
//using Company.Framework.Messaging.RabbitMq.Consumer.Settings;

//namespace Company.Framework.ExampleApi.Consumers;

//public class PingAppliedRabbitConsumer : CoreRabbitConsumer<Envelope<PingApplied>>
//{
//    private readonly ILogger _logger;

//    public PingAppliedRabbitConsumer(IRabbitConnectionContext connectionContext, RabbitConsumerSettings settings, ILogger<PingAppliedRabbitConsumer> logger) : base(connectionContext, settings)
//    {
//        _logger = logger;
//    }

//    protected override Task ConsumeAsync(Envelope<PingApplied> message, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("PingApplied event consumed, {}", message);
//        return Task.CompletedTask;
//    }
//}