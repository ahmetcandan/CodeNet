﻿using CodeNet.MongoDB;
using CodeNet.MongoDB.Models;

namespace StokTakip.Customer.Contract.Model;

[CollectionName("KeyValue")]
public class KeyValueModel : BaseMongoDBModel
{
    public string Key { get; set; }
    public string Value { get; set; }
}
