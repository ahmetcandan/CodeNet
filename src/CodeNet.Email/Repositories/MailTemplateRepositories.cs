using CodeNet.Email.Models;
using CodeNet.MongoDB.Repositories;

namespace CodeNet.Email.Repositories;

internal class MailTemplateRepositories(MongoDBContext dbContext) : BaseMongoRepository<MailTemplate>(dbContext)
{
}
