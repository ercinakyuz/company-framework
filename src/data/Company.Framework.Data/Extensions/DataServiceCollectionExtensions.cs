using Company.Framework.Data.Connection.Provider;
using Company.Framework.Data.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDbConnectionProvider(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<DbSettings>(configuration.GetSection("Db"))
                .AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
        }
    }
}
