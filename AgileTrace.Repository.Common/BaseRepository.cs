using System;
using System.Collections.Generic;
using System.Linq;
using AgileTrace.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbContext DbContext;
        public BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public T Delete(T entity)
        {
            var et = DbContext.Set<T>().Remove(entity).Entity;
            DbContext.SaveChanges();

            return et;
        }

        public IEnumerable<T> All()
        {
            return DbContext.Set<T>().ToList();
        }

        public T Get(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public T Insert(T entity)
        {
            var et = DbContext.Set<T>().Add(entity).Entity;
            DbContext.SaveChanges();

            return et;
        }

        public IEnumerable<T> Query(string sql,params object[] param)
        {
            return DbContext.Set<T>().FromSql(sql, param).ToList();
        }

        public T Update(T entity)
        {
            var et = DbContext.Set<T>().Update(entity).Entity;
            DbContext.SaveChanges();

            return et;
        }
    }
}
