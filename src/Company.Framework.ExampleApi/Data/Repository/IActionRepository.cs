using Company.Framework.Data.Repository;
using Company.Framework.ExampleApi.Data.Entity;

namespace Company.Framework.ExampleApi.Data.Repository;

public interface IActionRepository : IRepository<ActionEntity, Guid>
{
}

public interface IAction2Repository : IRepository<ActionEntity, Guid>
{
}
public interface IAction3Repository : IRepository<ActionEntity, Guid>
{
}

public interface IAction4Repository : IRepository<ActionEntity, Guid>
{
}

public interface IFooRepository : IRepository<Foo, int>
{
}