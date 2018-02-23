using AgileTrace.Configuration;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.Repository.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AgileTrace.Repository.Sql.DbContexts
{
    public class SqlserverDbContext :SqliteDbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conn = Config.AppSetting.store.connection;
            optionsBuilder.UseSqlServer(conn);
#if DEBUG
            var lf = new LoggerFactory();
            lf.AddProvider(new EfLoggerProvider());
            optionsBuilder.UseLoggerFactory(lf);
#endif
        }
    
    }
}
