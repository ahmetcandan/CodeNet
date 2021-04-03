using Net5Api.Abstraction;
using StokTakip.EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokTakip.Abstraction
{
    public interface IProductRepository : IRepository<Product>
    {
    }
}
