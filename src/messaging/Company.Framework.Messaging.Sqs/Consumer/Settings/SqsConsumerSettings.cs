namespace Company.Framework.Messaging.Sqs.Consumer.Settings;

public class SqsConsumerSettings
{
    public required string Queue { get; init; }

    public int Concurrency { get; init; } = 1;

    public void Deconstruct(out string queue, out int concurrency)
    {
        queue = Queue;
        concurrency = Concurrency;
    }
}