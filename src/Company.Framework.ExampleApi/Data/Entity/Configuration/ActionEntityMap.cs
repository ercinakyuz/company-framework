using Company.Framework.Data.Rdbms.Mappings;
using NHibernate.Mapping.ByCode;

namespace Company.Framework.ExampleApi.Data.Entity.Configuration;

public class ActionEntityMap : CoreEntityMap<Guid, ActionEntity>
{
    public ActionEntityMap()
    {
        Table("Action");
        Id(p => p.Id, map =>
        {
            map.Generator(Generators.Assigned);
        });
    }

}

public class FooEntityMap : CoreEntityMap<int, Foo>
{
    public FooEntityMap()
    {
        Table("Foo");
        Id(p => p.Id, map => map.Generator(Generators.Identity));
    }
}


