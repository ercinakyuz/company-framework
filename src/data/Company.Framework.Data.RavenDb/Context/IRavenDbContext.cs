using Company.Framework.Data.Db.Context;
using Raven.Client.Documents.Session;

namespace Company.Framework.Data.Raven.Context;

public interface IRavenDbContext : IDbContext
{
    IAsyncDocumentSession OpenSessionAsync();
}
