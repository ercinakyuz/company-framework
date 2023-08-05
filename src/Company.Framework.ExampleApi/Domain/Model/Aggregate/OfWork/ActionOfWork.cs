using Company.Framework.Domain.Model.Aggregate.OfWork;
using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.State;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.OfWork;


public class ActionOfWork : AggregateOfWork<IActionRepository, IActionConverter, Action, ActionId, ActionState, ActionEntity, Guid>, IActionOfWork
{
    public ActionOfWork(IActionRepository repository, IActionConverter converter) : base(repository, converter)
    {
    }
}