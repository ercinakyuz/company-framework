using Company.Framework.Data.Rdbms.Mappings;

namespace Company.Framework.ExampleApi.Data.Entity.Configuration
{
    public class ActionEntityMap : CoreEntityMap<Guid, ActionEntity>
    {
        public ActionEntityMap()
        {
            Table("Action");
            Id(p => p.Id).GeneratedBy.Assigned();
        }

    }

    public class FooEntityMap : CoreEntityMap<int, Foo>
    {
        public FooEntityMap()
        {
            Table("Foo");
            Id(p => p.Id).GeneratedBy.Identity();
        }
    }

}
