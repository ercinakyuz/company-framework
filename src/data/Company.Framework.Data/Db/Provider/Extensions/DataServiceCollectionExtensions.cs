using Company.Framework.Data.Db.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Db.Provider.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDbProvider(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<DbSettings>(configuration.GetSection("Db"))
                .AddSingleton<IDbProvider, DbProvider>();
        }
    }
}
