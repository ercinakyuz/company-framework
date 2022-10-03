using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;

public interface IActionBuilder
{
    Task<Action> BuildAsync(ActionId id, CancellationToken cancellationToken);
}