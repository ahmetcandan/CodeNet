using Microsoft.EntityFrameworkCore;
using Net5Api.Repository;
using StokTakip.Abstraction;
using StokTakip.EntityFramework.Models;
using System;

namespace StokTakip.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {

        }
    }
}
