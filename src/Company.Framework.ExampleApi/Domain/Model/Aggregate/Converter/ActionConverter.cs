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
            State = aggregate.State?.Value,
            Created = aggregate.Created,
            Modified = aggregate.Modified ?? new Core.Logging.Log()
        };
    }
}

public interface IActionConverter : IAggregateConverter<Action, ActionEntity>
{

}