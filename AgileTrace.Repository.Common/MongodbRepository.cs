using System;
using System.Collections.Generic;
using AgileTrace.Entity;
using AgileTrace.IRepository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgileTrace.Repository.Common
{
    public class MongoDbRepository<T>:IRepository<T> where T:IEntity,new ()
    {
        protected IMongoDatabase Database { get; set; }
        public MongoDbRepository(IMongoDatabase database)
        {
            Database = database;
        }

        public IMongoCollection<T> Collection
        {
            get { return Database.GetCollection<T>(CollectionName); }
        }

        public IEnumerable<T> All()
        {
            return Database.GetCollection<T>(CollectionName).Find(FilterDefinition<T>.Empty).ToList();
        }

        public T Get(object id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id.ToString());
            return Database.GetCollection<T>(CollectionName).Find(filter).FirstOrDefault();
        }

        public T Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            Database.GetCollection<T>(CollectionName).ReplaceOne(filter, entity);

            return entity;
        }

        public T Delete(T entity)
        {
            var filter = Builders<T>.Filter.Eq("_id", entity.Id);
            Database.GetCollection<T>(CollectionName).DeleteOne(filter) ;

            return entity;
        }

        public T Insert(T entity)
        {
            Database.GetCollection<T>(CollectionName).InsertOne(entity);

            return entity;
        }

        public IEnumerable<T> Query(string sql, params object[] param)
        {
            throw new NotImplementedException();
        }

        public string CollectionName
        {
            get
            {
                var t = typeof(T);
                return t.Name;
            }
        }
    }
}
