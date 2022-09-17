using Company.Framework.Core.Logging;
using Company.Framework.Data.Entity;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Data.Entity
{
    public record ActionEntity(
        Guid Id,
        Log Created,
        Log? Modified = default) : CoreEntity<Guid>(Id, Created, Modified);
}
