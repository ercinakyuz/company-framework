using Amazon.SQS;

namespace Company.Framework.Messaging.Sqs.Client.Context;

public record SqsClientContext(string BusName, IAmazonSQS Client) : ISqsClientContext
{
    public TClient Resolve<TClient>()
    {
        if (typeof(TClient) != typeof(IAmazonSQS))
            throw new InvalidOperationException("ClientContext type is not valid");
        return (TClient)Client;
    }
}