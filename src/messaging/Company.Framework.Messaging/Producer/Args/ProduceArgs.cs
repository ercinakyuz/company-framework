namespace Company.Framework.Messaging.Producer.Args;

public record ProduceArgs<TMessage>(string Channel, TMessage Message) where TMessage : notnull;