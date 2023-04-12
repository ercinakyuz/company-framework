namespace Company.Framework.Messaging.Sqs.Client.Context;

public interface ISqsClientContext
{
    string BusName { get; }
    TClient Resolve<TClient>();
}