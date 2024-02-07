using Company.Framework.Data.Entity;
using FluentNHibernate.Mapping;

namespace Company.Framework.Data.Rdbms.Mappings;

public abstract class CoreEntityMap<TId, TEntity> : ClassMap<TEntity> where TEntity : CoreEntity<TId>
{
    protected CoreEntityMap()
    {
        Map(p => p.State).Not.Nullable();
        Component(p => p.Created, m =>
        {
            m.Map(x => x.At, "Created_At").Not.Nullable();
            m.Map(x => x.By, "Created_By").Not.Nullable();
        });
        Component(p => p.Modified, m =>
        {
            m.Map(x => x.At, "Modified_At");
            m.Map(x => x.By, "Modified_By");
        });
    }
}
