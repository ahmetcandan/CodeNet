using CodeNet.MongoDB;

namespace StokTakip.Customer.Contract.Model;

[CollectionName("KeyValue")]
public class MongoModel
{
    public Guid _id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
}
