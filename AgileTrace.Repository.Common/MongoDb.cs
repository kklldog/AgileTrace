using AgileTrace.Configuration;
using MongoDB.Driver;

namespace AgileTrace.Repository.Common
{
    public static class MongoDb
    {
        public const string DbName = "AGILETRACE";

        public static MongoClient Client { get; }

        public static IMongoDatabase GetDatabase(string dbName = DbName)
        {
            return Client.GetDatabase(dbName);
        }

        static MongoDb()
        {
            string serverUrl = Config.AppSetting.store.connection;
            Client = new MongoClient(serverUrl);
        }
    }
}
