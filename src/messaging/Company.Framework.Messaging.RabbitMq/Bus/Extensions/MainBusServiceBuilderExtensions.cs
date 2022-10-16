using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.RabbitMq.Bus.Builder;

namespace Company.Framework.Messaging.RabbitMq.Bus.Extensions
{
    public static class MainBusServiceBuilderExtensions
    {
        public static RabbitBusBuilder WithRabbit(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return new RabbitBusBuilder(mainBusServiceBuilder).WithProviders();
        }
    }
}
