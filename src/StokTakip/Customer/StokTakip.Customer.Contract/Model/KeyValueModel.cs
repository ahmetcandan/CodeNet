using NetCore.Abstraction.Model;
using NetCore.Abstraction;

namespace StokTakip.Customer.Contract.Model;

[CollectionName("KeyValue")]
public class KeyValueModel : BaseMongoDBModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
