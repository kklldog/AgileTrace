using AgileTrace.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using AgileTrace.Entity;
using AgileTrace.Repository.Sqlite.Common;

namespace AgileTrace.Repository.Sqlite
{
    public class TraceRepository : BaseRepository<Trace>, ITraceRepository
    {
        public IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId, string level)
        {
            using (var db = NewDbContext)
            {
                var page = db.Traces.Where(t =>
                        (string.IsNullOrEmpty(appId) || t.AppId == appId)
                        && (string.IsNullOrEmpty(level) || t.Level == level))
                    .OrderByDescending(t => t.Time)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize);
                return page.ToList();
            }
        }

        public int Count(string appId, string level)
        {
            using (var db = NewDbContext)
            {
                var count = db.Traces.Count(t =>
                    (string.IsNullOrEmpty(appId) || t.AppId == appId)
                    && (string.IsNullOrEmpty(level) || t.Level == level));

                return count;
            }
        }

        public object GroupLevel(List<string> levels, string appId)
        {
            using (var db = NewDbContext)
            {
                var gp = db.Traces.Where(t => levels.Contains(t.Level)
                                              && (string.IsNullOrEmpty(appId) || t.AppId == appId))
                    .GroupBy(t => t.Level);
                var result = gp.Select(g => new
                {
                    name = g.Key,
                    value = g.Count()
                }).ToList();

                return result;
            }
        }
    }
}
