using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StokTakip.Product.Repository;

namespace StokTakip.Product.Container.Modules
{
    public class RepositoryModule : Module
    {
        public static void AddDbContext(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddEntityFrameworkSqlServer().AddDbContext<ProductDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Product")));
        }
    }
}
