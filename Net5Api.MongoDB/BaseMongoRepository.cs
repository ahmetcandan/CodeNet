using MongoDB.Driver;
using Net5Api.Abstraction;
using Net5Api.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Net5Api.MongoDB
{
    public class BaseMongoRepository<TModel> : INoSqlRepository<TModel>, INoSqlAsyncRepository<TModel> where TModel : INoSqlModel, new()
    {
        private readonly IMongoCollection<TModel> mongoCollection;

        public BaseMongoRepository(string collectionName)
        {
            var client = new MongoClient("mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass%20Community&ssl=false");
            var database = client.GetDatabase("Net5Api");
            mongoCollection = database.GetCollection<TModel>(collectionName);
        }

        public BaseMongoRepository(string mongoDBConnectionString, string dbName, string collectionName)
        {
            var client = new MongoClient(mongoDBConnectionString);
            var database = client.GetDatabase(dbName);
            mongoCollection = database.GetCollection<TModel>(collectionName);
        }

        public virtual List<TModel> GetList()
        {
            return mongoCollection.Find(c => true).ToList();
        }

        public virtual List<TModel> GetList(Expression<Func<TModel, bool>> filter)
        {
            return mongoCollection.Find(filter).ToList();
        }

        public virtual TModel GetById(string id)
        {
            return mongoCollection.Find(m => m.Id == id).FirstOrDefault();
        }

        public virtual TModel Create(TModel model)
        {
            mongoCollection.InsertOne(model);
            return model;
        }

        public virtual void Update(string id, TModel model)
        {
            mongoCollection.ReplaceOne(m => m.Id == id, model);
        }

        public virtual void Delete(TModel model)
        {
            mongoCollection.DeleteOne(m => m.Id == model.Id);
        }

        public virtual void Delete(string id)
        {
            mongoCollection.DeleteOne(m => m.Id == id);
        }

        public virtual bool ContainsId(string id)
        {
            return mongoCollection.Count(c => c.Id == id) > 0;
        }

        public virtual async Task<List<TModel>> GetListAsync()
        {
            return (await mongoCollection.FindAsync(c => true)).ToList();
        }

        public virtual async Task<List<TModel>> GetListAsync(Expression<Func<TModel, bool>> filter)
        {
            return (await mongoCollection.FindAsync(filter)).ToList();
        }

        public virtual async Task<TModel> GetByIdAsync(string id)
        {
            return (await mongoCollection.FindAsync(m => m.Id == id)).FirstOrDefault();
        }

        public virtual async Task<TModel> CreateAsync(TModel model)
        {
            await mongoCollection.InsertOneAsync(model);
            return model;
        }

        public virtual async Task UpdateAsync(string id, TModel model)
        {
            await mongoCollection.ReplaceOneAsync(m => m.Id == id, model);
        }

        public virtual async Task DeleteAsync(TModel model)
        {
            await mongoCollection.DeleteOneAsync(m => m.Id == model.Id);
        }

        public virtual async Task DeleteAsync(string id)
        {
            await mongoCollection.DeleteOneAsync(m => m.Id == id);
        }

        public virtual async Task<bool> ContainsIdAsync(string id)
        {
            return (await mongoCollection.CountAsync(c => c.Id == id) > 0);
        }
    }
}
