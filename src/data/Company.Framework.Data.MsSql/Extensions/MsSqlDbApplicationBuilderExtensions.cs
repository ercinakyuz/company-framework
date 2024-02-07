using Company.Framework.Data.Db.Provider;
using Company.Framework.Data.MySql.Context.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.MySql.Extensions
{
    public static class MsSqlDbApplicationBuilderExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder applicationBuilder)
        {
            var serviceProvider = applicationBuilder.ApplicationServices;
            var dbProvider = serviceProvider.GetRequiredService<IDbProvider>();
            var contexts = serviceProvider.GetRequiredService<IDbProvider>()
                .ResolveAll<IMsSqlDbContextProvider>().SelectMany(dbContextProvider => dbContextProvider.ResolveAll());
            foreach (var context in contexts)
            {
                context.Migrate();
            }
            return applicationBuilder;
        }
    }
}
