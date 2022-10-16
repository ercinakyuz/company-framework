namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitConsumerSettings
{
    public RabbitDeclarationArgs Declaration { get; init; }

    public void Deconstruct(out RabbitDeclarationArgs Declaration)
    {
        Declaration = this.Declaration;
    }
}