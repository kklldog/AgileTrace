using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Entity;
using AgileTrace.IRepository;

namespace AgileTrace.Services
{
    public interface IAppCache
    {
        App Get(string appId);
        void Set(App app);

        void Remove(string appId);
    }

    public class AppCache:IAppCache
    {
        private static readonly ConcurrentDictionary<string, App> Apps = new ConcurrentDictionary<string, App>();
        private readonly IAppRepository _appRepository;
        
        public AppCache(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        public App Get(string appId)
        {
            Apps.TryGetValue(appId, out App app);

            if (app != null)
            {
                return app;
            }

            app = _appRepository.Get(appId);
            if (app != null)
            {
                Apps.TryAdd(appId, app);
            }

            return app;
        }

        public void Set(App app)
        {
            Apps.TryRemove(app.Id, out App oapp);
            Apps.TryAdd(app.Id, app);
        }

        public void Remove(string appId)
        {
            Apps.TryRemove(appId, out App oapp);
        }
    }
}
