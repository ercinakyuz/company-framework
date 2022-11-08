using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;
using LanguageExt.Common;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Builder;

public interface IActionBuilder
{
    Task<Result<Action>> BuildAsync(ActionId id, CancellationToken cancellationToken);
}