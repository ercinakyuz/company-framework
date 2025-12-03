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
            return Task.WhenAll(_consumers.Select(c => c.SubscribeAsync(cancellationToken)));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(_consumers.Select(c => c.UnsubscribeAsync()));
        }
    }
}
