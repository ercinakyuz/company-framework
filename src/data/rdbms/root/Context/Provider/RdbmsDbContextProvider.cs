using System.Collections.Immutable;
using System.Reflection;
using Company.Framework.Data.Db.Settings;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace Company.Framework.Data.Rdbms.Context.Provider;

public class RdbmsDbContextProvider<TDialect> : IRdbmsDbContextProvider where TDialect : Dialect
{
    private readonly IReadOnlyDictionary<string, IRdbmsDbContext> _dbContextDictionary;

    public RdbmsDbContextProvider(DbProviderSettings settings)
    {
        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(context =>
            context.Key,
            context =>
            {
                var configuration = new Configuration();
                configuration.DataBaseIntegration(db =>
                {
                    db.ConnectionString = settings.Connection.String;
                    db.SchemaAction = SchemaAutoAction.Create;
                    db.Dialect<TDialect>();
                });

                AddClassMappingsFromAssembly(configuration, context.Assembly);

                var sessionFactory = configuration.BuildSessionFactory();
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

    private void AddClassMappingsFromAssembly(Configuration configuration, string assemblyName)
    {
        var modelMapper = new ModelMapper();
        modelMapper.AddMappings(Assembly.Load(assemblyName).GetTypes());
        configuration.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());
    }
}

//public class RdbmsDbContextProvider : IRdbmsDbContextProvider
//{
//    private readonly IReadOnlyDictionary<string, IRdbmsDbContext> _dbContextDictionary;

//    public RdbmsDbContextProvider(DbProviderSettings settings, IPersistanceConfigurer persistenceConfigurer)
//    {
//        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(
//            context => context.Key, context =>
//            {
//                var sessionFactory = Fluently.Configure()
//                .Database(persistenceConfigurer)
//                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load(context.Assembly)))
//                .ExposeConfiguration(c => new SchemaExport(c).Execute(true, true, false))
//                .BuildSessionFactory();
//                return (IRdbmsDbContext)new RdbmsDbContext(sessionFactory);
//            });
//    }

//    public IRdbmsDbContext Resolve(string key)
//    {
//        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
//            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
//        return dbContext;
//    }

//    public IEnumerable<IRdbmsDbContext> ResolveAll()
//    {
//        return _dbContextDictionary.Values;
//    }
//}