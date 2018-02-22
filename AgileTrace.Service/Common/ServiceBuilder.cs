using System;
using System.Collections.Generic;
using System.Text;
using AgileTrace.IRepository;
using AgileTrace.IService;
using Microsoft.Extensions.DependencyInjection;

namespace AgileTrace.Service.Common
{
    public static class ServiceBuilder
    {
        public static void AddBussinessService(this IServiceCollection services)
        {
            services.AddSingleton<IAppCache, AppCache>();
            services.AddSingleton<IWebsocketService, WebsocketService>();
        }
    }
}
