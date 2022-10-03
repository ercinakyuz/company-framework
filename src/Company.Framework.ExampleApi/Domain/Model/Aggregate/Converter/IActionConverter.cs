using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Converter;

public interface IActionConverter
{
    ActionEntity Convert(Action aggregate);
}