using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;

public class ActionConverter : IActionConverter
{
    public ActionEntity Convert(Action aggregate)
    {
        return new ActionEntity(aggregate.Id.Value, aggregate.State?.Value, aggregate.Created, aggregate.Modified);
    }
}