using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StokTakip.Customer.Repository;

namespace StokTakip.Customer.Container.Modules
{
    public class RepositoryModule : Module
    {
        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<CustomerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Customer")));
        }
    }
}
