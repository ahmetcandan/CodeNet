using System;

namespace Net5Api.Abstraction.Model
{
    public class CacheModel : INoSqlModel
    {
        public CacheModel()
        {

        }

        public CacheModel(string key, object value, int time)
        {
            Id = key;
            Value = value;
            ExpiryDate = DateTime.Now.AddSeconds(time);
        }

        public virtual string Id { get; set; }
        public virtual object Value { get; set; }
        public virtual DateTime ExpiryDate { get; set; }
    }
}
