using CodeNet.Email.Models;
using CodeNet.MongoDB;
using CodeNet.MongoDB.Repositories;

namespace CodeNet.Email.Repositories;

internal class OutboxRepositories(MongoDBContext dbContext) : BaseMongoRepository<Outbox>(dbContext)
{
}
