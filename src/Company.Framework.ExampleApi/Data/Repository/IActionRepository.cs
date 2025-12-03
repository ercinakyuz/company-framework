using Company.Framework.Data.Repository;
using Company.Framework.ExampleApi.Data.Entity;
using NUlid;

namespace Company.Framework.ExampleApi.Data.Repository;

public interface IActionRepository : IRepository<ActionEntity, Ulid>
{
}

public interface IAction2Repository : IRepository<ActionEntity, Ulid>
{
}
public interface IAction3Repository : IRepository<ActionEntity, Ulid>
{
}

public interface IAction4Repository : IRepository<ActionEntity, Ulid>
{
}

public interface IFooRepository : IRepository<Foo, int>
{
}