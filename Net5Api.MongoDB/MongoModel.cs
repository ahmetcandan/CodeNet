using System;
using MongoDB.Bson;

namespace Net5Api.MongoDB
{
    public abstract class BaseMongoModel
    {
        public ObjectId Id { get; set; }
    }
}
