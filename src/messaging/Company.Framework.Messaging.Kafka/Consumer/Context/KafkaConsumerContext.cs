﻿using Company.Framework.Messaging.Kafka.Consumer.Retrial.Context;
using Company.Framework.Messaging.Kafka.Consumer.Settings;
using Confluent.Kafka;

namespace Company.Framework.Messaging.Kafka.Consumer.Context
{
    public class KafkaConsumerContext<TMessage> : IKafkaConsumerContext
    {
        private readonly IConsumer<Ignore, TMessage> _consumer;

        public KafkaConsumerSettings Settings { get; }
        public IKafkaRetrialContext? Retrial { get; }

        public KafkaConsumerContext(IConsumer<Ignore, TMessage> consumer, KafkaConsumerSettings settings, IKafkaRetrialContext? consumerRetrialContext = default)
        {
            _consumer = consumer;
            Settings = settings;
            Retrial = consumerRetrialContext;
        }

        public TConsumer Resolve<TConsumer>()
        {
            if (typeof(TConsumer) != typeof(IConsumer<Ignore, TMessage>))
                throw new InvalidOperationException($"Producer type is not valid");
            return (TConsumer)_consumer;
        }
    }
}