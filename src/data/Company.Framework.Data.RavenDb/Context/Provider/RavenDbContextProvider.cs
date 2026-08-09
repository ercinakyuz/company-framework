using Company.Framework.Data.Db.Settings;
using Raven.Client.Documents;
using System.Collections.Immutable;

namespace Company.Framework.Data.Raven.Context.Provider;

public class RavenDbContextProvider : IRavenDbContextProvider
{
    private readonly IReadOnlyDictionary<string, IRavenDbContext> _dbContextDictionary;

    public RavenDbContextProvider(DbProviderSettings settings)
    {
        var documentStore = InitializeDocumentStore(settings);
        _dbContextDictionary = settings.Contexts.ToImmutableDictionary(
            context => context.Key, context => (IRavenDbContext)new RavenDbContext(documentStore, context.DbName));
    }

    public IRavenDbContext Resolve(string key)
    {
        if (!_dbContextDictionary.TryGetValue(key, out var dbContext))
            throw new EntryPointNotFoundException($"Db context does not exist for key: {key}");
        return dbContext;
    }

    public IEnumerable<IRavenDbContext> ResolveAll()
    {
        return _dbContextDictionary.Values;
    }

    private static IDocumentStore InitializeDocumentStore(DbProviderSettings settings)
    {
        var urls = settings.Connection.String.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var documentStore = new DocumentStore { Urls = urls };
        documentStore.Initialize();
        documentStore.Conventions.IdentityPartsSeparator = '-';
        documentStore.Conventions.UseOptimisticConcurrency = true;
        return documentStore;
    }
}
