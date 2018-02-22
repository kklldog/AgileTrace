using AgileTrace.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgileTrace.IRepository
{
    public interface ITraceRepository : IRepository<Trace>
    {
        IEnumerable<Trace> Page(int pageIndex, int pageSize, string appId,string level);

        int Count(string appId, string level);

        object GroupLevel(List<string> levels, string appId);
    }
}
