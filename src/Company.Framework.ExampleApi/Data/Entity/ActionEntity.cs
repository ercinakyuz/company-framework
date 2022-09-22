using Company.Framework.Core.Logging;
using Company.Framework.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Entity
{
    public record ActionEntity(
        Guid Id,
        Log Created,
        Log? Modified = default) : CoreEntity<Guid>(Id, Created, Modified);
}
