using CodeNet.MongoDB.Repositories;
using CodeNet.Outbox.Models;

namespace CodeNet.Outbox.Repositories;

internal class OutboxRepository(OutboxDbContext dbContext) : BaseMongoRepository<OutboxModel>(dbContext)
{
}
