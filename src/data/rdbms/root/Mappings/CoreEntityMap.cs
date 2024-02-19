using Company.Framework.Data.Entity;
using NHibernate.Mapping.ByCode.Conformist;

namespace Company.Framework.Data.Rdbms.Mappings;

public abstract class CoreEntityMap<TId, TEntity> : ClassMapping<TEntity> where TEntity : CoreEntity<TId>
{
    protected CoreEntityMap()
    {
        Property(p => p.State, map => map.NotNullable(true));
        Component(p => p.Created, m =>
        {
            m.Property(x => x.At, map =>
            {
                map.Column("Created_At");
                map.NotNullable(true);
            });
            m.Property(x => x.By, map =>
            {
                map.Column("Created_By");
                map.NotNullable(true);
            });
        });
        Component(p => p.Modified, m =>
        {
            m.Property(x => x.At, map =>
            {
                map.Column("Modified_At");
                map.NotNullable(false);
            });
            m.Property(x => x.By, map =>
            {
                map.Column("Modified_By");
                map.NotNullable(false);
            });
        });
    }
}
