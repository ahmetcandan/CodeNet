using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StokTakip.Product.Api.Controllers
{
    [ApiController]
    [Route("aspi/[controller]")]
    public class ProductController : ControllerBase
    {
        IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public List<ProductViewModel> GetAll()
        {
            return productService.GetProducts();
        }

        [HttpGet]
        public ProductViewModel Get(int productId)
        {
            return productService.GetProduct(productId);
        }

        [HttpPost]
        public ProductViewModel Post(ProductViewModel product)
        {
            return productService.CreateProduct(product);
        }

        [HttpPut]
        public ProductViewModel Put(ProductViewModel product)
        {
            return productService.UpdateProduct(product);
        }

        [HttpDelete]
        public ProductViewModel Delete(int productId)
        {
            return productService.DeleteProduct(productId);
        }
    }
}
