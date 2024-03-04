using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StokTakip.Product.Container.Modules
{
    public class RepositoryModule : Module
    {
        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<DbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        }
    }
}
