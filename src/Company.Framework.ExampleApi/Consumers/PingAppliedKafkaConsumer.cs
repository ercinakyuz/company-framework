//using Company.Framework.ExampleApi.Domain.Model.Aggregate.Event;
//using Company.Framework.Messaging.Envelope;
//using Company.Framework.Messaging.Kafka.Consumer;
//using Company.Framework.Messaging.Kafka.Consumer.Context;

//namespace Company.Framework.ExampleApi.Consumers
//{
//    public class PingAppliedKafkaConsumer : CoreKafkaConsumer<Envelope<PingApplied>>
//    {
//        public PingAppliedKafkaConsumer(IKafkaConsumerContext consumerContext, ILogger<PingAppliedKafkaConsumer> logger) : base(consumerContext, logger)
//        {
//        }

//        public override Task ConsumeAsync(Envelope<PingApplied> message, CancellationToken cancellationToken)
//        {
//            Logger.LogInformation("PingApplied event consumed, {}", message);
//            return Task.CompletedTask;
//        }
//    }
//}
