using Company.Framework.Messaging.Consumer;
using Microsoft.Extensions.Hosting;

namespace Company.Framework.Messaging.HostedServices
{
    public class ConsumersHostedService : IHostedService
    {
        private readonly IEnumerable<IConsumer> _consumers;

        public ConsumersHostedService(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                Task.Run(() => consumer.SubscribeAsync(cancellationToken), cancellationToken);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Unsubscribe();
            }
            return Task.CompletedTask;
        }
    }
}
