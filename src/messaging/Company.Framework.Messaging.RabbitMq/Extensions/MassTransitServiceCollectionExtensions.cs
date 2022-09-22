using Company.Framework.Core.Identity;
using Company.Framework.Messaging.RabbitMq.Producer;
using Company.Framework.Messaging.RabbitMq.Settings;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.RabbitMq.Extensions
{
    public static class MassTransitServiceCollectionExtensions
    {
        public static IServiceCollection AddBus(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<BusSettings>(configuration.GetSection("Bus"))
                .AddRabbitMq()
                .AddProducers();
        }

        public static IServiceCollection AddProducers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                    .AddTransient<IRabbitProducer, RabbitProducer>()
                //.AddScoped<ISendEndpointProvider, SendEndpointProvider>()
                ;
        }
        public static IServiceCollection AddRabbitMq(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddMassTransit(registrationConfigurator =>
            {
                registrationConfigurator.AddConsumer<InputQueueConsumer>();
                registrationConfigurator.UsingInMemory((context, configurator) =>
                {
                    configurator.ReceiveEndpoint("input-queue", e =>
                    {
                        // delegate consumer factory
                        e.Consumer(() => new InputQueueConsumer());
                        configurator.ConfigureEndpoints(context);

                        //configurator.ConfigureEndpoints(context);

                    });
                });
                //registrationConfigurator.UsingRabbitMq((context, factoryConfigurator) =>
                //{
                //    var busSettings = context.GetRequiredService<IOptions<BusSettings>>().Value;
                //    var connectionSettings = busSettings.Connection;
                //    factoryConfigurator.Host(connectionSettings.Host, hostConfigurator =>
                //    {
                //        hostConfigurator.Username(connectionSettings.Username);
                //        hostConfigurator.Password(connectionSettings.Password);
                //        var clusterNodes = connectionSettings.ClusterNodes;
                //        if (clusterNodes.Any())
                //            hostConfigurator.UseCluster(clusterConfigurator =>
                //            {
                //                clusterNodes.ForEach(clusterConfigurator.Node);
                //            });
                //    });
                //    factoryConfigurator.ReceiveEndpoint();
                //    factoryConfigurator.ConfigureEndpoints(context);
                //    busSettings.Consumers.ForEach(consumerSettings =>
                //    {
                //        factoryConfigurator.ReceiveEndpoint(consumerSettings.Queue, endpointConfigurator =>
                //        {
                //            registrationConfigurator.AddConsumer(Type.GetType(consumerSettings.QualifiedName));
                //        });
                //    });
                //});
            });
        }
    }

    //public class InputQueueConsumer : IConsumer<Envelope<PingApplied>>
    //{
    //    public Task Consume(ConsumeContext<Envelope<PingApplied>> context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public class InputQueueConsumer : IConsumer<object>
    {
        public async Task Consume(ConsumeContext<object> context)
        {
            Console.WriteLine("asdkjasld");
            await Task.CompletedTask;
        }
    }

    public record PingApplied(CoreId<Guid> AggregateId);
}
