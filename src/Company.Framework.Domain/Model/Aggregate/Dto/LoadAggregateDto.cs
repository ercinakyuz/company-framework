using Company.Framework.Core.Identity;
using Company.Framework.Core.Logging;

namespace Company.Framework.Domain.Model.Aggregate.Dto;

public abstract record LoadAggregateDto<TId>(TId Id, Log Created, Log? Modified) where TId : CoreId<TId>;