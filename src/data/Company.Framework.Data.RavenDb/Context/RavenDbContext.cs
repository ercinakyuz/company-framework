using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Company.Framework.Data.Raven.Context;

public class RavenDbContext : IRavenDbContext
{
    private readonly IDocumentStore _documentStore;
    private readonly string _dbName;

    public RavenDbContext(IDocumentStore documentStore, string dbName)
    {
        _documentStore = documentStore;
        this._dbName = dbName;
    }

    public IAsyncDocumentSession OpenSessionAsync()
    {
        return _documentStore.OpenAsyncSession(_dbName);
    }
}
