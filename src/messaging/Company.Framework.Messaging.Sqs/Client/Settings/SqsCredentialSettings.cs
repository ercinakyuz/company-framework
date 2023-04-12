namespace Company.Framework.Messaging.Sqs.Client.Settings;

public class SqsCredentialSettings
{
    public required string AccessKey { get; init; }

    public required string SecretKey { get; init; }
}