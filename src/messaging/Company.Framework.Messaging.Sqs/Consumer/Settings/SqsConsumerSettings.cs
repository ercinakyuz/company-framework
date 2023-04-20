namespace Company.Framework.Messaging.Sqs.Consumer.Settings;

public class SqsConsumerSettings
{
    public required string Queue { get; init; }

    public int Concurrency { get; init; } = 1;

    public IReadOnlyList<string> AttributeNames { get; init; } = new List<string> { "All" };

    public void Deconstruct(out string queue, out int concurrency, out List<string> attributeNames)
    {
        queue = Queue;
        concurrency = Concurrency;
        attributeNames = AttributeNames.ToList();
    }
}