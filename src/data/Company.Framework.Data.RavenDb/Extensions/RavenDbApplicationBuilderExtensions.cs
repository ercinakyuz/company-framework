using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents;

namespace Company.Framework.Data.Raven.Extensions;

public static class RavenDbApplicationBuilderExtensions
{
    public static IApplicationBuilder UseRavenDb(this IApplicationBuilder app)
    {
        var documentStore = app.ApplicationServices.GetRequiredService<IDocumentStore>();

        // Ensure indexes are created and persisted
        documentStore.Maintenance.Send(new Raven.Client.Documents.Operations.Indexes.GetIndexesOperation(0, 128));

        return app;
    }
}
