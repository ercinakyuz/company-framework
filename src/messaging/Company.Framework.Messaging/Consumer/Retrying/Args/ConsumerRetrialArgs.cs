namespace Company.Framework.Messaging.Consumer.Retrying.Args
{
    public record ConsumerRetrialArgs(object Message, object Attributes, Type ExceptionType);
}
