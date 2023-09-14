﻿using System.Runtime.CompilerServices;
using Company.Framework.Core.Monad;
using Company.Framework.Core.Monad.Extensions;
using Company.Framework.ExampleApi.Data.Entity;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using static Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder.Error.ActionBuilderError;

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
            return (await _actionRepository.FindAsync(id.Value))
                .Map(FromEntity)
                .ToResult(() => ActionNotFound);
        }

        public async IAsyncEnumerable<Action> BuildAsyncEnumerable(IEnumerable<ActionId> ids, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var entity in _actionRepository.FindAllAsync(e => ids.Contains(ActionId.From(e.Id))).WithCancellation(cancellationToken))
            {
                yield return FromEntity(entity);
            }
        }

        private static Action FromEntity(ActionEntity entity)
        {
            return Action.Load(new LoadActionDto(ActionId.From(entity.Id), entity.Created, entity.Modified));
        }
    }
}
