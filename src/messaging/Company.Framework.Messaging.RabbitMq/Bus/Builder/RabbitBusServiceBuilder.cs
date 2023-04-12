using Company.Framework.Core.Serializer;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.RabbitMq.Connection;
using Company.Framework.Messaging.RabbitMq.Connection.Context;
using Company.Framework.Messaging.RabbitMq.Connection.Context.Provider;
using Company.Framework.Messaging.RabbitMq.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer.Context;
using Company.Framework.Messaging.RabbitMq.Consumer.Retrying.Handler;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Company.Framework.Messaging.RabbitMq.Producer;
using Company.Framework.Messaging.RabbitMq.Producer.Provider;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using static Company.Framework.Messaging.Constant.MessagingConstants;

namespace Company.Framework.Messaging.RabbitMq.Bus.Builder;

public class RabbitBusServiceBuilder : CoreBusServiceBuilder<RabbitBusBuilder>
{
    private const string RabbitPrefix = "Messaging:Rabbit";
    private const string BusPrefix = $"{RabbitPrefix}:Buses";
    private readonly string _namedBusPrefix;

    public RabbitBusServiceBuilder(RabbitBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
        _namedBusPrefix = $"{BusPrefix}:{BusName}";
    }
    internal RabbitBusServiceBuilder WithConnectionContext()
    {
        ServiceCollection.AddSingleton<IRabbitConnectionContext>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSection($"{_namedBusPrefix}:Connection").Get<RabbitConnectionSettings>();
            var connectionFactory = new ConnectionFactory
            {
                DispatchConsumersAsync = true
            };
            if (settings?.Port != null)
                connectionFactory.Port = settings.Port.Value;

            var connection = settings?.Nodes != null
                ? connectionFactory.CreateConnection(settings.Nodes.Split(";"))
                : connectionFactory.CreateConnection();
            return new RabbitConnectionContext(BusName, connection);
        });
        return this;
    }
    internal RabbitBusServiceBuilder WithDefaultProducer()
    {
        return WithProducer(DefaultProducerName);
    }
    public RabbitBusServiceBuilder WithProducer(string name)
    {
        ServiceCollection.AddSingleton<IRabbitProducer, RabbitProducer>(serviceProvider =>
        {
            var connectionContext = serviceProvider.GetRequiredService<IRabbitConnectionContextProvider>().Resolve(BusName);
            var jsonSerializer = serviceProvider.GetRequiredService<IJsonSerializer>();
            return new RabbitProducer(name, BusName, connectionContext, jsonSerializer);
        });
        return this;
    }
    public RabbitBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name, ConsumerRetriability? retriability = default)
        where TConsumer : CoreRabbitConsumer<TMessage>
    {
        ServiceCollection.AddSingleton<IConsumer, TConsumer>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionContext = serviceProvider.GetRequiredService<IRabbitConnectionContextProvider>().Resolve(BusName);
            var jsonSerializer = serviceProvider.GetRequiredService<IJsonSerializer>();
            var consumerSection = configuration.GetSection($"{_namedBusPrefix}:Consumers:{name}");
            var settings = consumerSection.Get<RabbitConsumerSettings>();
            IRabbitConsumerRetryingHandler? retryingHandler = default;
            if (retriability != default)
            {
                var consumerDeclaration = settings.Declaration;
                var rabbitRetrySettings = consumerSection.GetSection("Retry").Get<RabbitRetrySettings>();
                rabbitRetrySettings.Declaration = new RabbitDeclarationArgs
                {
                    Exchange = new RabbitExchangeArgs { Name = $"retry_{consumerDeclaration.Exchange.Name}", Type = ExchangeType.Topic },
                    Routing = consumerDeclaration.Routing,
                    Queue = $"retry_{consumerDeclaration.Queue}"
                };
                var producer = serviceProvider.GetRequiredService<IRabbitProducerContextProvider>().Resolve(BusName).Default();
                retryingHandler = ActivatorUtilities.CreateInstance<RabbitConsumerRetryingHandler>(serviceProvider, producer, retriability, rabbitRetrySettings);
            }

            var context = new RabbitConsumerContext(connectionContext, settings, jsonSerializer, retryingHandler);
            return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, context);
        });
        return this;
    }

    public RabbitBusServiceBuilder ThatConsume<TMessage>(string name, ConsumerRetriability? retriability = default)
        where TMessage : INotification
    {
        return WithConsumer<DefaultRabbitConsumer<TMessage>, TMessage>(name, retriability);
    }
}