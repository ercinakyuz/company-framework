using MassTransit;

namespace Company.Framework.Messaging.RabbitMq.Producer
{
    public class RabbitProducer : IRabbitProducer
    {
        private readonly IBus _bus;
        //private readonly ISendEndpointProvider _sendEndpointProvider;

        public RabbitProducer(IBus bus)
        {
            _bus = bus;
            //_sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Produce<TMessage>(string queue, TMessage message) where TMessage : notnull
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri("queue:input-queue"));

            await endpoint.Send(message);
        }
    }


}
