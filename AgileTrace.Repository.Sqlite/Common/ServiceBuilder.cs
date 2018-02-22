using System;
using System.Collections.Generic;
using System.Text;
using AgileTrace.IRepository;
using Microsoft.Extensions.DependencyInjection;

namespace AgileTrace.Repository.Sqlite.Common
{
    public static class ServiceBuilder
    {
        public static void AddSqliteRepository(this IServiceCollection services)
        {
            services.AddSingleton<IAppRepository, AppRepository>();
            services.AddSingleton<ITraceRepository, TraceRepository>();

            new TraceDbContext().InitTables();
        }
    }
}
