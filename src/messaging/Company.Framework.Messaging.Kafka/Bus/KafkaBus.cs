using Company.Framework.Messaging.Bus;
using Company.Framework.Messaging.Kafka.Producer.Context;
using Company.Framework.Messaging.Kafka.Producer.Context.Provider;

namespace Company.Framework.Messaging.Kafka.Bus
{
    public class KafkaBus : CoreBus, IKafkaBus
    {
        public IKafkaProducerContext ProducerContext { get; }
        public ITypedKafkaProducerContext TypedProducerContext { get; }

        public KafkaBus(string name, IKafkaProducerContextProvider producerContextProvider, ITypedKafkaProducerContextProvider typedProducerContextProvider) : base(name)
        {
            ProducerContext = producerContextProvider.Resolve(name);
            TypedProducerContext = typedProducerContextProvider.Resolve(name);
        }
    }
}
