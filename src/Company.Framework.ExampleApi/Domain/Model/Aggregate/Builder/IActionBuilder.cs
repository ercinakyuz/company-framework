using Company.Framework.Core.Monad;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;

public interface IActionBuilder
{
    Task<Result<Action>> BuildAsync(ActionId id, CancellationToken cancellationToken);

    IAsyncEnumerable<Action> BuildAllStreaming(IEnumerable<ActionId> ids, CancellationToken cancellationToken);

    IAsyncEnumerable<Action> BuildAllStreaming(CancellationToken cancellationToken);

    Task<IEnumerable<Action>> BuildAllAsync(CancellationToken cancellationToken);
}