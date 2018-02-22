using AgileTrace.Entity;
using AgileTrace.Repository.Sqlite.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Repository.Sqlite
{
    public class TraceDbContext:DbContext
    {
        public DbSet<Trace> Traces { get; set; }
        public DbSet<App> Apps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=agiletrace.db");
#if DEBUG
            var lf = new LoggerFactory();
            lf.AddProvider(new EfLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
#endif

        }

        public void InitTables()
        {
            this.Database.EnsureCreated();
        }
    }
}
