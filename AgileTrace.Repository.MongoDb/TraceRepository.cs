using System.Collections.Generic;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.Repository.Common;
using AgileTrace.Repository.MongoDb.DbContexts;
using MongoDB.Driver;

namespace AgileTrace.Repository.MongoDb
{
    public class TraceRepository : MongoDbRepository<Trace>, ITraceRepository
    {

        public TraceRepository(IMongoContext dbContext) : base(dbContext.Database)
        {
        }

        public IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId, string level)
        {
            var sk = (pageIndex - 1) * pageSize;
            var page = Collection.Find(t =>
                      (string.IsNullOrEmpty(appId) || t.AppId == appId)
                      && (string.IsNullOrEmpty(level) || t.Level == level))
                .Sort(new SortDefinitionBuilder<Trace>().Descending(t => t.Time))
                .Skip(sk)
                .Limit(pageSize)
                .ToList();

            return page;
        }

        public int Count(string appId, string level)
        {
            return (int)Collection.Count(t => (string.IsNullOrEmpty(level) || t.AppId == appId) && (string.IsNullOrEmpty(level) || t.Level == level));
        }

        public object GroupLevel(List<string> levels, string appId)
        {
            var result = new List<object>();
            foreach (var level in levels)
            {
                var value = Count(appId, level);
                result.Add(new
                {
                    name = level,
                    value
                });
            }

            return result;
        }

    }
}
