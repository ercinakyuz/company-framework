using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Bus.Provider;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Company.Framework.Messaging.RabbitMq.Producer;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Company.Framework.Messaging.RabbitMq.Bus.Builder;

public class RabbitBusServiceBuilder : CoreBusServiceBuilder<RabbitBusBuilder>
{
    public RabbitBusServiceBuilder(RabbitBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
    }

    internal RabbitBusServiceBuilder WithDefaultBus()
    {
        ServiceCollection.AddSingleton<IBus>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionFactory = new ConnectionFactory { HostName = configuration.GetSection($"Messaging:{BusName}:Host").Value };
            var connection = connectionFactory.CreateConnection();
            return new RabbitBus(BusName, connection);
        });
        return this;
    }
    internal RabbitBusServiceBuilder WithDefaultProducer()
    {
        ServiceCollection.AddSingleton<IRabbitProducer, RabbitProducer>(serviceProvider =>
        {
            var bus = serviceProvider.GetRequiredService<IBusProvider>().Resolve<IRabbitBus>(BusName);
            return new RabbitProducer(BusName, bus);
        });
        return this;
    }

    public RabbitBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name)
        where TConsumer : CoreRabbitConsumer<TMessage>
    {
        ServiceCollection.AddSingleton<IConsumer, TConsumer>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var bus = serviceProvider.GetRequiredService<IBusProvider>().Resolve<IRabbitBus>(BusName);
            var settings = BuildRabbitConsumerSettings(name, configuration);
            return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, bus, settings);
        });
        return this;
    }

    public RabbitBusServiceBuilder ThatConsume<TMessage>(string name)
        where TMessage : INotification
    {
        return WithConsumer<DefaultRabbitConsumer<TMessage>, TMessage>(name);
    }

    private RabbitConsumerSettings BuildRabbitConsumerSettings(string name, IConfiguration configuration)
    {
        var consumerConfigKey = $"Messaging:{BusName}:Consumers:{name}";
        var settings = new RabbitConsumerSettings(Exchange: new RabbitExchangeSettings(
                Name: configuration.GetSection($"{consumerConfigKey}:Exchange:Name").Value,
                Type: configuration.GetSection($"{consumerConfigKey}:Exchange:Type").Value),
            Routing: configuration.GetSection($"{consumerConfigKey}:Routing").Value,
            Queue: configuration.GetSection($"{consumerConfigKey}:Queue").Value);
        return settings;
    }


}