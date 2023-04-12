namespace Company.Framework.Messaging.Sqs.Client.Context.Provider;

public interface ISqsClientContextProvider
{
    ISqsClientContext Resolve(string busName);
}