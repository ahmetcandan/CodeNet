using CodeNet.MongoDB.Extensions;
using CodeNet.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeNet.Email.Extensions;

public class MailServiceOptionsBuilder(IServiceCollection services)
{
    private bool _hasMongoDb = false;

    public MailServiceOptionsBuilder AddMongoDB(IConfigurationSection configuration)
    {
        services.AddMongoDB(configuration);
        _hasMongoDb = true;
        return this;
    }

    public MailServiceOptionsBuilder AddMongoDB(MongoDbOptions mongoDbOptions)
    {
        services.AddMongoDB(mongoDbOptions);
        _hasMongoDb = true;
        return this;
    }

    public bool HasMongoDB { get { return _hasMongoDb; } }
}
