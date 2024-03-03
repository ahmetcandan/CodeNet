using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StokTakip.Campaign.Repository;

namespace StokTakip.Campaign.Container.Modules
{
    public class RepositoryModule : Module
    {
        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<CampaignDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Customer")));
        }
    }
}
