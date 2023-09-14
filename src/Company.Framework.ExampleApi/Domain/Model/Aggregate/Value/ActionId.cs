using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Core.Id.Implementations;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Value
{
    public record ActionId(Guid Value) : IdOfGuid<ActionId>(Value), IId<ActionId, Guid>
    {
        //public static ActionId From(Guid value)
        //{
        //    return new ActionId(value);
        //}
    }
}
