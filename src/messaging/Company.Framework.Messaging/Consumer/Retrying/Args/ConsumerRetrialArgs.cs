namespace Company.Framework.Messaging.Consumer.Retrying.Args
{
    public record ConsumerRetrialArgs(object Message, object Headers, Type ExceptionType);
}
