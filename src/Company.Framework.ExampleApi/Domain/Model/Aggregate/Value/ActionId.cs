using Company.Framework.Core.Id.Implementations;
using NUlid;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Value
{
    public record ActionId(Ulid Value) : IdOfUlid<ActionId>(Value);
}
