using Company.Framework.Core.Error;
using Company.Framework.Domain.Model.Exception;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using LanguageExt.Common;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder
{
    public class ActionBuilder : IActionBuilder
    {
        private readonly IActionRepository _actionRepository;

        public ActionBuilder(IActionRepository actionRepository)
        {
            _actionRepository = actionRepository;
        }

        public async Task<Result<Action>> BuildAsync(ActionId id, CancellationToken cancellationToken)
        {
            return (await _actionRepository.GetByIdAsync(id.Value))
                .Match(
                 entity => new Result<Action>(Action.Load(new LoadActionDto(ActionId.From(entity.Id), entity.Created, entity.Modified))),
                 () => new Result<Action>(new AggregateBuilderException(new CoreError("ACDBE-1", "Action not found")))
                );
        }
    }
}
