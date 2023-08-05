using Company.Framework.Aspect.Processors;

namespace Company.Framework.Domain.Model.Aggregate.OfWork.Processors;

public abstract class AggregateOfWorkPostProcessor : PostProcessor<AggregateRoot> { }

public abstract class BatchAggregateOfWorkPostProcessor : PostProcessor<IEnumerable<AggregateRoot>> { }