namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitConsumerSettings
{
    public string Exchange { get; set; }

    public string Routing { get; set; }

    public string Queue { get; set; }
}