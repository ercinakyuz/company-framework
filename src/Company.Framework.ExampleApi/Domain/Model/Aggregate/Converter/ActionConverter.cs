using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Converter;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;

public class ActionConverter : IActionConverter
{
    public ActionEntity Convert(Action aggregate)
    {
        return new ActionEntity
        {
            Id = aggregate.Id.Value,
            State = aggregate.State.Value,
            Created = aggregate.Created,
            Modified = aggregate.Modified ?? Log.Load(aggregate.Created.By)
        };
    }
}

public interface IActionConverter : IAggregateConverter<Action, ActionEntity>
{

}