using Company.Framework.Data.Extensions;
using Company.Framework.Data.Mongo.Extensions;
using Company.Framework.ExampleApi.Data.Repository;

namespace Company.Framework.ExampleApi.Data.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataComponents(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDbConnectionProvider(configuration)
                .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMongoDb()
                .AddMongoRepository<IActionRepository, ActionRepository>("task-management-instance", "task-management-context");
        }
    }
}
