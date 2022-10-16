using Company.Framework.Messaging.Consumer.Settings;

namespace Company.Framework.Messaging.RabbitMq.Consumer.Settings;

public class RabbitRetrySettings : CoreRetrySettings
{
    public RabbitDeclarationArgs Declaration { get; set; }

}