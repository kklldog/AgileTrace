using AgileTrace.IRepository;
using System;
using AgileTrace.Entity;
using Microsoft.EntityFrameworkCore;
using AgileTrace.Repository.Common;

namespace AgileTrace.Repository.Sql
{
    public class AppRepository : SqlRepository<App>, IAppRepository
    {
        public AppRepository(IDbContext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}
