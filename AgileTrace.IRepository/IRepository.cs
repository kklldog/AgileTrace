using System;
using System.Collections.Generic;
using AgileTrace.Entity;

namespace AgileTrace.IRepository
{
    public interface IRepository<T> where T : IEntity
    {
        IEnumerable<T> All();
        T Get(object id);

        T Update(T entity);

        T Delete(T entity);

        T Insert(T entity);

        IEnumerable<T> Query(string sql, params object[] param);
    }
}
