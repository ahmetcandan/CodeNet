using CodeNet.MongoDB.Repositories;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Options;

namespace CodeNet.Outbox.Models;

internal class OutboxDbContext(IOptions<MongoDbOptions<OutboxDbContext>> options) : MongoDBContext(options)
{
}
