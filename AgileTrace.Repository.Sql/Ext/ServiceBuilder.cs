using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgileTrace.Configuration;
using AgileTrace.IRepository;
using AgileTrace.Repository.Sql.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace AgileTrace.Repository.Sql.Ext
{
    public static class ServiceBuilder
    {
        public static IServiceCollection AddSqlRepository(this IServiceCollection services)
        {
            string storeType = Config.AppSetting.store.type;
            if (storeType=="sqlite")
            {
                services.AddScoped<IDbContext, SqliteDbContext>();
                new SqliteDbContext().InitTables();
            }
            if (storeType == "sqlserver")
            {
                services.AddScoped<IDbContext, SqlserverDbContext>();
                new SqlserverDbContext().InitTables();
            }
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<ITraceRepository, TraceRepository>();

            return services;
        }
    }
}
