using Net5Api.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StokTakip.Service
{
    public class ProductService : IProductService
    {
        IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        List<Product> IProductService.GetProducts()
        {
            return productRepository.GetAll().ToList();
        }
    }
}
