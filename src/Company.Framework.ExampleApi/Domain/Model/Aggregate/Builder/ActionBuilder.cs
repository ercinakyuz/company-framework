using Company.Framework.Core.Logging;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder
{
    public class ActionBuilder
    {
        public Action Build(ActionId id)
        {
            return Action.Load(new LoadActionDto(id, Log.Load("Creator"), Log.Load("Modifier")));
        }
    }
}
