using System;
using System.Collections.Generic;
using System.Text;
using AgileTrace.IRepository;
using MongoDB.Driver;

namespace AgileTrace.Repository.MongoDb.DbContexts
{
    public interface IMongoContext:IDbContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext: IMongoContext
    {
        public IMongoDatabase Database =>
            Common.MongoDb.GetDatabase();

        public void InitTables()
        {
             //
        }
    }
}
