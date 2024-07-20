using Company.Framework.Core.Logging;
using Company.Framework.Domain.Model.Aggregate.Dto;

namespace Company.Framework.ExampleApi.Domain.Model.Dto;

public record PingActionDto(Log Created) : CreateAggregateDto(Created);
