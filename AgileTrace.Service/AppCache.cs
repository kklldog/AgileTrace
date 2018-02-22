using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.IService;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;

namespace AgileTrace.Service
{
    public class AppCache : IAppCache
    {
        private readonly IAppRepository _appRepository;
        private readonly IMemoryCache _memoryCache;

        public AppCache(IAppRepository appRepository, IMemoryCache memoryCache)
        {
            _appRepository = appRepository;
            _memoryCache = memoryCache;
        }

        public App Get(string appId)
        {
            var key = $"app_{appId}";
            _memoryCache.TryGetValue(key, out App app);

            if (app != null)
            {
                return app;
            }

            app = _appRepository.Get(appId);
            if (app != null)
            {
                _memoryCache.Set(key, app);
            }

            return app;
        }

        public void Set(App app)
        {
            var key = $"app_{app.Id}";
            _memoryCache.Set(key, app);
        }

        public void Remove(string appId)
        {
            var key = $"app_{appId}";
            _memoryCache.Remove(key);
        }
    }
}
