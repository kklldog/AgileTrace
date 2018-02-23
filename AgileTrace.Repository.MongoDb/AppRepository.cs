using AgileTrace.Entity;
using AgileTrace.IRepository;
using AgileTrace.Repository.Common;
using AgileTrace.Repository.MongoDb.DbContexts;

namespace AgileTrace.Repository.MongoDb
{
    public class AppRepository :MongoDbRepository<App>, IAppRepository
    {
        public AppRepository(IMongoContext dbContext) : base(dbContext.Database)
        {

        }
    }
}
