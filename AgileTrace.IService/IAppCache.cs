using System;
using AgileTrace.Entity;

namespace AgileTrace.IService
{
    public interface IAppCache
    {
        App Get(string appId);
        void Set(App app);

        void Remove(string appId);
    }
}
