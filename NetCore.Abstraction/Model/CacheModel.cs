using ServiceStack.Text;
using System;

namespace NetCore.Abstraction.Model
{
    [RuntimeSerializable]
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
