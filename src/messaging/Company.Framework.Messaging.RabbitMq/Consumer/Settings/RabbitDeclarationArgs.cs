namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitDeclarationArgs
{
    public required RabbitExchangeArgs Exchange { get; init; }
    public required string Routing { get; init; }
    public required string Queue { get; init; }

    public void Deconstruct(out RabbitExchangeArgs exchange, out string routing, out string queue)
    {
        exchange = Exchange;
        routing = Routing;
        queue = Queue;
    }

}