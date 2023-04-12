using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Company.Framework.Core.Serializer;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Consumer;
using Company.Framework.Messaging.Consumer.Retrying;
using Company.Framework.Messaging.Sqs.Client;
using Company.Framework.Messaging.Sqs.Client.Context;
using Company.Framework.Messaging.Sqs.Client.Context.Provider;
using Company.Framework.Messaging.Sqs.Consumer;
using Company.Framework.Messaging.Sqs.Consumer.Context;
using Company.Framework.Messaging.Sqs.Consumer.Retrying.Handler;
using Company.Framework.Messaging.Sqs.Consumer.Settings;
using Company.Framework.Messaging.Sqs.Producer;
using Company.Framework.Messaging.Sqs.Producer.Provider;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Company.Framework.Messaging.Constant.MessagingConstants;

namespace Company.Framework.Messaging.Sqs.Bus.Builder;

public class SqsBusServiceBuilder : CoreBusServiceBuilder<SqsBusBuilder>
{
    private const string SqsPrefix = "Messaging:Sqs";
    private const string BusPrefix = $"{SqsPrefix}:Buses";
    private readonly string _namedBusPrefix;

    public SqsBusServiceBuilder(SqsBusBuilder busBuilder, string busName) : base(busBuilder, busName)
    {
        _namedBusPrefix = $"{BusPrefix}:{BusName}";
    }
    internal SqsBusServiceBuilder WithClientContext()
    {
        ServiceCollection.AddSingleton<ISqsClientContext>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var settings = configuration.GetSection($"{_namedBusPrefix}:Client").Get<SqsClientSettings>();
            var credentials = new BasicAWSCredentials("notValidKey", "notValidKey");
            var amazonSqsConfig = new AmazonSQSConfig
            {
                ServiceURL = "http://localhost:9324",
            };

            var client = new AmazonSQSClient(credentials, amazonSqsConfig);

            return new SqsClientContext(BusName, client);
        });
        return this;
    }
    internal SqsBusServiceBuilder WithDefaultProducer()
    {
        return WithProducer(DefaultProducerName);
    }
    public SqsBusServiceBuilder WithProducer(string name)
    {
        ServiceCollection.AddSingleton<ISqsProducer, SqsProducer>(serviceProvider =>
        {
            var connectionContext = serviceProvider.GetRequiredService<ISqsClientContextProvider>().Resolve(BusName);
            var jsonSerializer = serviceProvider.GetRequiredService<IJsonSerializer>();
            return new SqsProducer(name, BusName, connectionContext, jsonSerializer);
        });
        return this;
    }
    public SqsBusServiceBuilder WithConsumer<TConsumer, TMessage>(string name, ConsumerRetriability? retriability = default)
        where TConsumer : CoreSqsConsumer<TMessage>
    {
        ServiceCollection.AddSingleton<IConsumer, TConsumer>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionContext = serviceProvider.GetRequiredService<ISqsClientContextProvider>().Resolve(BusName);
            var consumerSection = configuration.GetSection($"{_namedBusPrefix}:Consumers:{name}");
            var settings = consumerSection.Get<SqsConsumerSettings>();
            ISqsConsumerRetryingHandler? retryingHandler = default;
            if (retriability != default)
            {
                var retrySettings = consumerSection.GetSection("Retry").Get<SqsRetrySettings>();
                retrySettings.Consumer = new SqsConsumerSettings
                {
                    Queue = $"retry_{settings.Queue}",
                    Concurrency = settings.Concurrency
                };
                var producer = serviceProvider.GetRequiredService<ISqsProducerContextProvider>().Resolve(BusName).Default();
                retryingHandler = ActivatorUtilities.CreateInstance<SqsConsumerRetryingHandler>(serviceProvider, producer, retriability, retrySettings);
            }
            var jsonSerializer = serviceProvider.GetRequiredService<IJsonSerializer>();
            var context = new SqsConsumerContext(connectionContext, settings,jsonSerializer, retryingHandler);
            return ActivatorUtilities.CreateInstance<TConsumer>(serviceProvider, context);
        });
        return this;
    }

    public SqsBusServiceBuilder ThatConsume<TMessage>(string name, ConsumerRetriability? retriability = default)
        where TMessage : INotification
    {
        return WithConsumer<DefaultSqsConsumer<TMessage>, TMessage>(name, retriability);
    }
}