using System;
using AgileTrace.Repository.Entity;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository
{
    public class TraceDbContext:DbContext
    {
        public DbSet<Trace> Traces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=agiletrace.db");
        }

        public void InitTables()
        {
            this.Database.EnsureCreated();
        }
    }
}
