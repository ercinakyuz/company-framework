using Company.Framework.Data.Db.Provider.Extensions;
using Company.Framework.Data.Mongo.Extensions;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.Data.Rdbms.Extensions;
using Company.Framework.Data.Repository.Extensions;

namespace Company.Framework.ExampleApi.Data.Extensions
{
    public static class DataServiceCollectionExtensions
    {
        public static IServiceCollection AddDataComponents(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddDbProvider(configuration)
                .AddRepositories();
            return serviceCollection;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMongoRepositories()
                .AddMsSqlRepositories();
        }

        private static IServiceCollection AddMongoRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMongoDb();
            //.AddMongoRepository<IActionRepository, ActionMongoRepository>(new RepositorySettings("task-management-instance", "task-management-context", "actions"));
        }

        private static IServiceCollection AddMsSqlRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMsSqlDb()
                .AddMsSqlRepository<IActionRepository, ActionMsSqlRepository>(new RepositorySettings("task-management-mssql-instance", "task-management-context"))
                .AddMsSqlRepository<IFooRepository, FooMsSqlRepository>(new RepositorySettings("task-management-mssql-instance", "task-management-context"));
        }
    }
}
