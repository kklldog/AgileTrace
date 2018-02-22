using System;
using System.Collections.Generic;
using System.Linq;
using AgileTrace.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository.Sqlite.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public TraceDbContext NewDbContext
        {
            get
            {
                return new TraceDbContext();
            }
        }

        public void Delete(T entity)
        {
            using (NewDbContext)
            {
                NewDbContext.Set<T>().Remove(entity);
                NewDbContext.SaveChanges();
            }
        }

        public IEnumerable<T> All()
        {
            using (NewDbContext)
            {
                return NewDbContext.Set<T>().ToList();
            }
        }

        public T Get(object id)
        {
            using (NewDbContext)
            {
                return NewDbContext.Set<T>().Find(id);
            }
        }

        public T Insert(T entity)
        {
            using (NewDbContext)
            {
                NewDbContext.Set<T>().Add(entity);
                NewDbContext.SaveChanges();

                return entity;
            }
        }

        public IEnumerable<T> Query(string sql,params object[] param)
        {
            using (NewDbContext)
            {
               return NewDbContext.Set<T>().FromSql(sql, param).ToList();
            }
        }

        public void Update(T entity)
        {
            using (NewDbContext)
            {
                NewDbContext.Set<T>().Update(entity);
                NewDbContext.SaveChanges();
            }
        }
    }
}
