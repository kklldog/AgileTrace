using AgileTrace.Configuration;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Repository.Sql.DbContexts
{
    public class SqliteDbContext : DbContext, IDbContext
    {
        public DbSet<Trace> Traces { get; set; }
        public DbSet<App> Apps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = Config.AppSetting.store.connection;
            optionsBuilder.UseSqlite(conn);

#if DEBUG
            var lf = new LoggerFactory();
            lf.AddProvider(new EfLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
#endif
        }

        public virtual void InitTables()
        {
            this.Database.EnsureCreated();
        }

    }
}
