using Company.Framework.Aspect.Processors;

namespace Company.Framework.Domain.Model.Aggregate.OfWork.Processors;

public abstract class AggregateOfWorkPreProcessor : PreProcessor<AggregateRoot> { }

public abstract class BatchAggregateOfWorkPreProcessor : PreProcessor<IEnumerable<AggregateRoot>> { }

