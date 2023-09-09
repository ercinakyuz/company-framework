﻿using Company.Framework.Domain.Model.Aggregate.State;

namespace Company.Framework.ExampleApi.Domain.Model.Aggregate.State;

public record ActionState(string Value) : CoreState<ActionState>
{
    public static readonly ActionState PingApplied = new("PingApplied");

    public static readonly ActionState PongApplied = new("PongApplied");
}