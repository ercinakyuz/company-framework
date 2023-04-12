namespace Company.Framework.Messaging.Sqs.Client.Settings
{
    public class SqsClientSettings
    {
        public string? ServiceUrl { get; init; }

        public string? Region { get; init; }
        public SqsCredentialSettings? Credentials { get; init; }
    }
}
