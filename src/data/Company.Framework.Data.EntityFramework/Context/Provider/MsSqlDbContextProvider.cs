using System.Collections.Immutable;
using Company.Framework.Data.Db.Settings;

namespace Company.Framework.Data.EntityFramework.Context.Provider;

public class MsSqlDbContextProvider : IMsSqlDbContextProvider
{
    private readonly IReadOnlyDictionary<string, IMsSqlDbContext> _dbContextDictionary;

    public MsSqlDbContextProvider(DbProviderSettings settings)
    {
        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(
            context => context.Key, context => (IMsSqlDbContext)new MsSqlDbContext(settings.Connection));
    }

    public IMsSqlDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }

    public IEnumerable<IMsSqlDbContext> ResolveAll()
    {
        return _dbContextDictionary.Values;
    }
}