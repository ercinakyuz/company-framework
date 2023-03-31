using Company.Framework.Core.Monad;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;

public interface IActionBuilder
{
    Task<Result<Action>> BuildAsync(ActionId id, CancellationToken cancellationToken);

    IAsyncEnumerable<Action> BuildAsyncEnumerable(IEnumerable<ActionId> ids, CancellationToken cancellationToken);
}