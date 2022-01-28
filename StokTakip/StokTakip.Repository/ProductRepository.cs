using Microsoft.EntityFrameworkCore;
using NetCore.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;

namespace StokTakip.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {

        }
    }
}
