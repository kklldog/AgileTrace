using System;
using System.Collections.Generic;
using System.Text;
using AgileTrace.IRepository;
using Microsoft.Extensions.DependencyInjection;

namespace AgileTrace.Repository.Sqlite.Common
{
    public static class ServiceBuilder
    {
        public static IServiceCollection AddSqliteRepository(this IServiceCollection services)
        {
            services.AddScoped<ISqliteDbContext, SqliteDbContext>();
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<ITraceRepository, TraceRepository>();

            new SqliteDbContext().InitTables();

            return services;
        }
    }
}
