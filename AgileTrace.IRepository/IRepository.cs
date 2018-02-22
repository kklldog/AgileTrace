using System;
using System.Collections.Generic;

namespace AgileTrace.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> All();
        T Get(object id);

        void Update(T entity);

        void Delete(T entity);

        T Insert(T entity);

        IEnumerable<T> Query(string sql, params object[] param);
    }
}
