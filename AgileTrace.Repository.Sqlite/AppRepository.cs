using AgileTrace.IRepository;
using System;
using AgileTrace.Entity;
using AgileTrace.Repository.Sqlite.Common;

namespace AgileTrace.Repository.Sqlite
{
    public class AppRepository : BaseRepository<App>, IAppRepository
    {
    }
}
