using AgileTrace.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgileTrace.IRepository
{
    public interface ITraceRepository : IRepository<Trace>
    {
        IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId,string level,DateTime startDate,DateTime endDate);

        int Count(string appId, string level, DateTime startDate, DateTime endDate);

        List<dynamic> GroupLevel(List<string> levels, string appId,DateTime startDate, DateTime endDate);
    }
}
