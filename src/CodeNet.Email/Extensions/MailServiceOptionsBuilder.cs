using CodeNet.MongoDB.Extensions;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Email.Extensions;

public class MailServiceOptionsBuilder(IServiceCollection services)
{
    public MailServiceOptionsBuilder AddMongoDB(IConfigurationSection configuration)
    {
        services.AddMongoDB(configuration);
        HasMongoDB = true;
        return this;
    }

    public MailServiceOptionsBuilder AddMongoDB(MongoDbOptions mongoDbOptions)
    {
        services.AddMongoDB(mongoDbOptions);
        HasMongoDB = true;
        return this;
    }

    public bool HasMongoDB { get; private set; } = false;
}
