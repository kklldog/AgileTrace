using AgileTrace.Configuration;
using AgileTrace.IRepository;
using AgileTrace.Repository.MongoDb.DbContexts;
using Microsoft.Extensions.DependencyInjection;

namespace AgileTrace.Repository.MongoDb.Ext
{
    public static class ServiceBuilder
    {
        public static IServiceCollection AddMongoDbRepository(this IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IAppRepository, AppRepository>();
            services.AddScoped<ITraceRepository, TraceRepository>();

            return services;
        }
    }
}
