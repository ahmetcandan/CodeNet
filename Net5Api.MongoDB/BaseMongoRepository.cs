using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net5Api.MongoDB
{
    public class BaseMongoRepository<TModel> where TModel : BaseMongoModel
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
            //var docId = new ObjectId(hexadecimalToString(id));
            return mongoCollection.Find<TModel>(m => m.Id == id).FirstOrDefault();
        }

        public virtual TModel Create(TModel model)
        {
            mongoCollection.InsertOne(model);
            return model;
        }

        public virtual void Update(string id, TModel model)
        {
            //var docId = new ObjectId(hexadecimalToString(id));
            mongoCollection.ReplaceOne(m => m.Id == id, model); 
        }

        public virtual void Delete(TModel model)
        {
            mongoCollection.DeleteOne(m => m.Id == model.Id);
        }

        public virtual void Delete(string id)
        {
            //var docId = new ObjectId(hexadecimalToString(id));
            mongoCollection.DeleteOne(m => m.Id == id);
        }

        public virtual bool ContainsId(string id)
        {
            //var docId = new ObjectId(hexadecimalToString(id));
            return mongoCollection.Find(c => c.Id == id).Any();
        }

        private string hexadecimalToString(string key)
        {
            return BitConverter.ToString(Encoding.Default.GetBytes(key)).Replace("-", "");
        }
    }
}
