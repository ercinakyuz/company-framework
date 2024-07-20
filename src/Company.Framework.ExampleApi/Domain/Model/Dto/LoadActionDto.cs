using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Dto;
using Company.Framework.ExampleApi.Domain.Model.Aggregate.Value;

namespace Company.Framework.ExampleApi.Domain.Model.Dto;

public record LoadActionDto(ActionId Id, Log Created, Log? Modified) : LoadAggregateDto<ActionId>(Id, Created, Modified);
