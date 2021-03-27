using Net5Api.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework;
using StokTakip.Model;
using System;

namespace StokTakip.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(StokTakipContext context) : base(context)
        {

        }
    }
}
