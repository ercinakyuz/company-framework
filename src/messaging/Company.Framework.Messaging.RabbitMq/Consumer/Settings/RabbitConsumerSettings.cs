namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public record RabbitConsumerSettings(RabbitExchangeSettings Exchange, string Routing, string Queue);