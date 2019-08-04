﻿using System.Collections.Generic;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.Repository.Common;
using AgileTrace.Repository.MongoDb.DbContexts;
using MongoDB.Driver;
using System;
using System.Dynamic;

namespace AgileTrace.Repository.MongoDb
{
    public class TraceRepository : MongoDbRepository<Trace>, ITraceRepository
    {

        public TraceRepository(IMongoContext dbContext) : base(dbContext.Database)
        {
        }

        public IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId, string level, DateTime startDate, DateTime endDate)
        {
            var sk = (pageIndex - 1) * pageSize;
            var page = Collection.Find(t =>
                      (string.IsNullOrEmpty(appId) || t.AppId == appId)
                      && (string.IsNullOrEmpty(level) || t.Level == level)
                      && t.Time >= startDate
                      && t.Time < endDate)
                .Sort(new SortDefinitionBuilder<Trace>().Descending(t => t.Time))
                .Skip(sk)
                .Limit(pageSize)
                .ToList();

            return page;
        }

        public int Count(string appId, string level, DateTime startDate, DateTime endDate)
        {
            return (int)Collection.Count(t => (string.IsNullOrEmpty(level) || t.AppId == appId)
                                        && (string.IsNullOrEmpty(level) || t.Level == level)
                                        && t.Time >= startDate
                                        && t.Time < endDate);
        }

        private int Count(string appId, string level)
        {
            return (int)Collection.Count(t => (string.IsNullOrEmpty(level) || t.AppId == appId)
                                        && (string.IsNullOrEmpty(level) || t.Level == level)
                                        );
        }

        public List<dynamic> GroupLevel(List<string> levels, string appId, DateTime startDate, DateTime endDate)
        {
            var result = new List<dynamic>();
            foreach (var level in levels)
            {
                var value = Count(appId, level, startDate, endDate);
                if (value > 0)
                {
                    dynamic obj = new ExpandoObject();
                    obj.name = level;
                    obj.value = value;
                    result.Add(obj);
                }
            }

            return result;
        }

    }
}
