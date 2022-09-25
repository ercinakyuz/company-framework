using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Bus.Provider;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer;
using Company.Framework.Messaging.RabbitMq.Consumer.Settings;
using Company.Framework.Messaging.RabbitMq.Producer;
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
            var model = connection.CreateModel();
            return new RabbitBus(BusName, model);
        });
        return this;
    }
    internal RabbitBusServiceBuilder WithDefaultProducer()
    {
        ServiceCollection.AddSingleton<IRabbitProducer, RabbitProducer>(serviceProvider =>
        {
            var bus = serviceProvider.GetRequiredService<IBusProvider>().Resolve<RabbitBus>(BusName);
            return new RabbitProducer(BusName, bus.Model);
        });
        return this;
    }

    public RabbitBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name)
        where TConsumer : RabbitConsumer<TMessage>
    {
        ServiceCollection.AddSingleton<IConsumer, TConsumer>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var bus = serviceProvider.GetRequiredService<IBusProvider>().Resolve<RabbitBus>(BusName);
            var consumerConfigKey = $"Messaging:{BusName}:Consumers:{name}";
            var settings = new RabbitConsumerSettings
            {
                Exchange = configuration.GetSection($"{consumerConfigKey}:Exchange").Value,
                Routing = configuration.GetSection($"{consumerConfigKey}:Routing").Value,
                Queue = configuration.GetSection($"{consumerConfigKey}:Queue").Value
            };
            return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, bus.Model, settings);
        });
        return this;
    }


}