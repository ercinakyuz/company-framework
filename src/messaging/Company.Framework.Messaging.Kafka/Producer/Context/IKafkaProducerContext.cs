﻿using Company.Framework.Messaging.Producer.Context;

namespace Company.Framework.Messaging.Kafka.Producer.Context;

public interface IKafkaProducerContext: IProducerContext
{
    public IKafkaProducer Resolve(string name);
}