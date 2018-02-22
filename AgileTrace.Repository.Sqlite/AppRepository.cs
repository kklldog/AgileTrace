using AgileTrace.IRepository;
using System;
using AgileTrace.Entity;
using AgileTrace.Repository.Sqlite.Common;
using Microsoft.EntityFrameworkCore;

namespace AgileTrace.Repository.Sqlite
{
    public class AppRepository : BaseRepository<App>, IAppRepository
    {
        public AppRepository(ISqliteDbContext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}
