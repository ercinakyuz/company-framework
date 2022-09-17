using Company.Framework.Data.Core.Provider;
using Company.Framework.Data.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.Framework.Data.Core.Extensions
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
