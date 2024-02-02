using Company.Framework.Core.Logging;
using Company.Framework.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Entity
{
    //public record ActionEntity(Guid Id, string State, Log Created, Log? Modified = null)
    //    : CoreEntity<Guid>(Id, State, Created, Modified);

    public class ActionEntity : CoreEntity<Guid>
    {
    }
}
