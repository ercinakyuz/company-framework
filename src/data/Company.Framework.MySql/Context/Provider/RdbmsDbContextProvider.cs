using System.Collections.Immutable;
using System.Reflection;
using Company.Framework.Data.Db.Settings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Company.Framework.Data.Rdbms.Context.Provider;

public class RdbmsDbContextProvider : IRdbmsDbContextProvider
{
    private readonly IReadOnlyDictionary<string, IRdbmsDbContext> _dbContextDictionary;

    public RdbmsDbContextProvider(DbProviderSettings settings, IPersistenceConfigurer persistenceConfigurer)
    {
        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(
            context => context.Key, context =>
            {
                var sessionFactory = Fluently.Configure()
                .Database(persistenceConfigurer)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(context.Assembly)))
                //.ExposeConfiguration(c => new SchemaExport(c).Execute(true, true, false))
                .BuildSessionFactory();
                return (IRdbmsDbContext)new RdbmsDbContext(sessionFactory);
            });
    }

    public IRdbmsDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }

    public IEnumerable<IRdbmsDbContext> ResolveAll()
    {
        return _dbContextDictionary.Values;
    }
}