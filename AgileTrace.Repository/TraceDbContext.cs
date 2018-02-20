using System;
using AgileTrace.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Repository
{
    public class TraceDbContext:DbContext
    {
        public DbSet<Trace> Traces { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<App> Apps { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=agiletrace.db");
            var lf = new LoggerFactory();
            lf.AddProvider(new EfLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
        }

        public void InitTables()
        {
            this.Database.EnsureCreated();
        }
    }
}
