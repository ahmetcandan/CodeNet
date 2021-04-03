using System;
using System.Text;
using MongoDB.Bson;

namespace Net5Api.MongoDB
{
    public abstract class BaseMongoModel
    {
        public virtual string Id { get; set; }

        public string CreateObjectId(string id)
        {
            return id;
            //return new ObjectId(BitConverter.ToString(Encoding.Default.GetBytes(id)).Replace("-", ""));
        }
    }
}
