using Company.Framework.Data.Db.Provider.Extensions;
using Company.Framework.Data.Mongo.Extensions;
using Company.Framework.ExampleApi.Data.Repository;
using Company.Framework.Data.Repository.Extensions;
using Company.Framework.Data.Rdbms.Extensions;
using Company.Framework.Data.Rdbms.MsSql.Extensions;
using Company.Framework.Data.Rdbms.MySql.Extensions;
using Company.Framework.Data.Rdbms.PostgreSql.Extensions;

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
                .AddMsSqlRepositories()
                .AddMySqlRepositories()
                .AddPostgreSqlRepositories();
        }

        private static IServiceCollection AddMsSqlRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMsSqlDb()
                .AddRdbmsRepository<IActionRepository, ActionRdbmsRepository>(new RepositorySettings("task-management-mssql-instance", "task-management-context"))
                .AddRdbmsRepository<IFooRepository, FooRdbmsRepository>(new RepositorySettings("task-management-mssql-instance", "task-management-context"));
        }

        private static IServiceCollection AddMySqlRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMySqlDb()
                .AddRdbmsRepository<IAction2Repository, ActionRdbmsRepository>(new RepositorySettings("task-management-mysql-instance", "task-management-context"))
                .AddRdbmsRepository<IFooRepository, FooRdbmsRepository>(new RepositorySettings("task-management-mysql-instance", "task-management-context"));
        }

        private static IServiceCollection AddPostgreSqlRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddPostgreSqlDb()
                .AddRdbmsRepository<IAction3Repository, ActionRdbmsRepository>(new RepositorySettings("task-management-postgresql-instance", "task-management-context"))
                .AddRdbmsRepository<IFooRepository, FooRdbmsRepository>(new RepositorySettings("task-management-postgresql-instance", "task-management-context"));
        }

        private static IServiceCollection AddMongoRepositories(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddMongoDb()
                .AddMongoRepository<IAction4Repository, ActionMongoRepository>(new RepositorySettings("task-management-mongo-instance", "task-management-context", "actions"));
        }
    }
}
