using Company.Framework.Messaging.Bus.Builder;
using Company.Framework.Messaging.Sqs.Bus.Builder;

namespace Company.Framework.Messaging.Sqs.Bus.Extensions
{
    public static class MainBusServiceBuilderExtensions
    {
        public static SqsBusBuilder WithSqs(this MainBusServiceBuilder mainBusServiceBuilder)
        {
            return new SqsBusBuilder(mainBusServiceBuilder).WithProviders();
        }
    }
}
