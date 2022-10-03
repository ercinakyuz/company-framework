using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder
{
    public class ActionBuilder : IActionBuilder
    {
        private readonly IActionRepository _actionRepository;

        public ActionBuilder(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }

        public async Task<Action> BuildAsync(ActionId id, CancellationToken cancellationToken)
        {
            var actionEntity = await _actionRepository.GetByIdAsync(id.Value);
            return actionEntity is default(ActionEntity)
                ? throw new InvalidOperationException("Action not found")
                : Action.Load(new LoadActionDto(ActionId.From(actionEntity.Id), actionEntity.Created,
                    actionEntity.Modified));
        }
    }
}
