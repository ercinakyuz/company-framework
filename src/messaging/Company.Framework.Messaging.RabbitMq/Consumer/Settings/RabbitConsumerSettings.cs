namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitConsumerSettings
{
    public required RabbitDeclarationArgs Declaration { get; init; }

    public void Deconstruct(out RabbitDeclarationArgs declaration)
    {
        declaration = Declaration;
    }
}