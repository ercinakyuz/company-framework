using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Sqs.Client.Context.Provider;
using Company.Framework.Messaging.Sqs.Producer.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Messaging.Sqs.Bus.Builder;

public class SqsBusBuilder : CoreBusBuilder<SqsBusBuilder>
{
    public SqsBusBuilder(MainBusServiceBuilder mainBusServiceBuilder) : base(mainBusServiceBuilder)
    {
    }
    public SqsBusServiceBuilder WithBus(string busName)
    {
        ServiceCollection.AddSingleton<IBus>(serviceProvider => ActivatorUtilities.CreateInstance<SqsBus>(serviceProvider, busName));
        return new SqsBusServiceBuilder(this, busName).WithClientContext().WithDefaultProducer();
    }

    internal SqsBusBuilder WithProviders()
    {
        ServiceCollection
            .AddSingleton<ISqsClientContextProvider, SqsClientContextProvider>()
            .AddSingleton<ISqsProducerContextProvider, SqsProducerContextProvider>();
        return this;
    }
    public MainBusServiceBuilder BuildSqs()
    {
        return Build();
    }
}