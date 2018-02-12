using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTrace.Repository;
using AgileTrace.Repository.Entity;

namespace AgileTrace.Services
{
    public class AppService
    {
        private static readonly ConcurrentDictionary<string, App> Apps = new ConcurrentDictionary<string, App>();

        public static App Get(string appId)
        {
            Apps.TryGetValue(appId, out App app);

            if (app != null)
            {
                return app;
            }

            using (var db = new TraceDbContext())
            {
                app = db.Apps.FirstOrDefault(a => a.Id == appId);
                if (app != null)
                {
                    Apps.TryAdd(appId, app);
                }
            }

            return app;
        }
    }
}
