using System;
using System.Collections.Generic;
using System.Linq;
using AgileTrace.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository.Sqlite.Common
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        public TraceDbContext NewDbContext()
        {
            return new TraceDbContext();
        }

        public T Delete(T entity)
        {
            using (var db = NewDbContext())
            {
                var et = db.Set<T>().Remove(entity).Entity;
                db.SaveChanges();

                return et;
            }
        }

        public IEnumerable<T> All()
        {
            using (var db = NewDbContext())
            {
                return db.Set<T>().ToList();
            }
        }

        public T Get(object id)
        {
            using (var db = NewDbContext())
            {
                return db.Set<T>().Find(id);
            }
        }

        public T Insert(T entity)
        {
            using (var db = NewDbContext())
            {
               var et = db.Set<T>().Add(entity).Entity;
                db.SaveChanges();

                return et;
            }
        }

        public IEnumerable<T> Query(string sql,params object[] param)
        {
            using (var db = NewDbContext())
            {
               return db.Set<T>().FromSql(sql, param).ToList();
            }
        }

        public T Update(T entity)
        {
            using (var db = NewDbContext())
            {
                var et = db.Set<T>().Update(entity).Entity;
                db.SaveChanges();

                return et;
            }
        }
    }
}
