﻿using CodeNet.Abstraction;
using StokTakip.Customer.Abstraction.Repository;
using StokTakip.Customer.Contract.Model;

namespace StokTakip.Customer.Repository;

public class KeyValueMongoRepository(MongoDBContext dbContext) : BaseMongoRepository<KeyValueModel>(dbContext), IKeyValueRepository
{
}
