using Company.Framework.Data.Db.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Company.Framework.Data.EntityFramework.Context
{
    public class MsSqlDbContext : DbContext, IMsSqlDbContext
    {
        private readonly DbConnectionSettings _dbConnectionSettings;

        public MsSqlDbContext(DbConnectionSettings dbConnectionSettings)
        {
            _dbConnectionSettings = dbConnectionSettings;
        }

        public void Migrate()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbConnectionSettings.String, b => b.MigrationsAssembly("Company.Framework.Api")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Array.ForEach(AppDomain.CurrentDomain.GetAssemblies(), asembly => modelBuilder.ApplyConfigurationsFromAssembly(asembly));
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var rowCount = await base.SaveChangesAsync();
            ChangeTracker.Clear();
            return rowCount;
        }


    }

}
