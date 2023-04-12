using Company.Framework.Core.Identity;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Value
{
    public class ActionId : CoreId<ActionId, Guid>
    {
        public ActionId(Guid value) : base(value)
        {
        }

        public ActionId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}
