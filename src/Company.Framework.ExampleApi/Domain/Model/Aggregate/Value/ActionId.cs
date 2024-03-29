﻿using Company.Framework.Core.Id.Implementations;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.Value
{
    public record ActionId(Guid Value) : IdOfGuid<ActionId>(Value)
    {
    }
}
