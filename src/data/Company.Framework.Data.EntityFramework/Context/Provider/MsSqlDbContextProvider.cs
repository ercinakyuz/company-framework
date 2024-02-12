using System.Collections.Concurrent;
using Company.Framework.Data.Db.Settings;
using Microsoft.EntityFrameworkCore;

namespace Company.Framework.Data.EntityFramework.Context.Provider;

public class MsSqlDbContextProvider : IMsSqlDbContextProvider
{
    private readonly IDictionary<string, IMsSqlDbContext> _dbContextDictionary;

    public MsSqlDbContextProvider(DbProviderSettings settings)
    {
        _dbContextDictionary = new ConcurrentDictionary<string, IMsSqlDbContext>();

        Array.ForEach(settings.Contexts, context =>
        {
            var modelBuilder = new ModelBuilder();
            Array.ForEach(AppDomain.CurrentDomain.GetAssemblies(), assembly => modelBuilder.ApplyConfigurationsFromAssembly(assembly));
            var dbContextOptions = new DbContextOptionsBuilder()
            .UseModel(modelBuilder.FinalizeModel())
            .UseSqlServer(settings.Connection.String)
            .Options;
            _dbContextDictionary[context.Key] = new MsSqlDbContext(new DbContext(dbContextOptions));
        });
    }

    public IMsSqlDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }
}