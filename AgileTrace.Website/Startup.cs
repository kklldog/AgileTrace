using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Configuration;
using AgileTrace.Middleware;
using AgileTrace.Repository.MongoDb.Ext;
using AgileTrace.Repository.Sql.Ext;
using AgileTrace.Service.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AgileTrace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.LoginPath = "/Login";
            });
            services.AddMemoryCache();
            services.AddMvc();
            AddRepository(services);
            services.AddBussinessService();
        }

        private void AddRepository(IServiceCollection services)
        {
            string storeType = Config.AppSetting.store.type;
            if (storeType == "sqlite" || storeType == "sqlserver")
            {
                services.AddSqlRepository();
            }
            else if (storeType == "mongodb")
            {
                services.AddMongoDbRepository();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            });
            app.UseMiddleware<WebSocketHandlerMiddleware>();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
