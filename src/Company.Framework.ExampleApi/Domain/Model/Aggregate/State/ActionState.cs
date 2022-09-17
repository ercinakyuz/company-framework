﻿using Company.Framework.Domain.Model.Aggregate.State;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.State;

public record ActionState(string Value) : CoreState<ActionState>(Value)
{
    public static readonly ActionState PingApplied = new("PingApplied");
}