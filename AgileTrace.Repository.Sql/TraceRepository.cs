using AgileTrace.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AgileTrace.Configuration;
using AgileTrace.Entity;
using AgileTrace.Repository.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository.Sql
{
    public class TraceRepository : SqlRepository<Trace>, ITraceRepository
    {
        public TraceRepository(IDbContext dbContext) : base(dbContext as DbContext)
        {
        }

        public IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId, string level, DateTime startDate, DateTime endDate)
        {
            var page = DbContext.Set<Trace>().Where(t =>
                    (string.IsNullOrEmpty(appId) || t.AppId == appId)
                    && (string.IsNullOrEmpty(level) || t.Level == level)
                    && t.Time >= startDate
                    && t.Time < endDate)
                .OrderByDescending(t => t.Time)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
            return page.ToList();
        }

        public int Count(string appId, string level, DateTime startDate, DateTime endDate)
        {
            var count = DbContext.Set<Trace>().Count(t =>
                (string.IsNullOrEmpty(appId) || t.AppId == appId)
                && (string.IsNullOrEmpty(level) || t.Level == level)
                && t.Time >= startDate
                && t.Time < endDate);

            return count;
        }

        public object GroupLevel(List<string> levels, string appId)
        {
            var result = new List<object>();
            StringBuilder sql = new StringBuilder("select level,count(1) as amount from Traces t where 1=1 ");
            if (levels != null)
            {
                var arr = levels.Select(l => string.Format("'{0}'", l)).ToArray();
                var inConditon = string.Join(',', arr);
                sql.AppendFormat(" and t.level in ({0}) ", inConditon);
            }
            if (!string.IsNullOrEmpty(appId))
            {
                sql.Append(" and t.AppId = @appId ");
            }
            sql.Append(" group by level ");
            using (var conn = DbContext.Database.GetDbConnection())
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql.ToString();
                if (conn is SqlConnection)
                {
                    cmd.Parameters.Add(new SqlParameter("@appId", appId));
                }
                if (conn is SqliteConnection)
                {
                    cmd.Parameters.Add(new SqliteParameter("@appId", appId));
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var name = reader.GetString(0);
                        var value = reader.GetInt32(1);

                        result.Add(new { name = name, value = value });
                    }

                    return result;
                }
            }
        }

    }
}
