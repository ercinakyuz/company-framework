namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitDeclarationArgs
{
    public RabbitExchangeArgs Exchange { get; init; }
    public string Routing { get; init; }
    public string Queue { get; init; }

    public void Deconstruct(out RabbitExchangeArgs Exchange, out string Routing, out string Queue)
    {
        Exchange = this.Exchange;
        Routing = this.Routing;
        Queue = this.Queue;
    }

}