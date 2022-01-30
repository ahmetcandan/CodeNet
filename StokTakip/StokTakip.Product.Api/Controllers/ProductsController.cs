using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;

namespace StokTakip.Product.Api.Controllers
{
    [Authorize(Roles = "product")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        IProductService ProductService;

        public ProductsController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public List<ProductViewModel> GetAll()
        {
            ProductService.SetUser(User);
            return ProductService.GetProducts();
        }

        [HttpGet("{productId}")]
        public ProductViewModel Get(int productId)
        {
            ProductService.SetUser(User);
            return ProductService.GetProduct(productId);
        }

        [HttpPost]
        public ProductViewModel Post(ProductViewModel product)
        {
            ProductService.SetUser(User);
            return ProductService.CreateProduct(product);
        }

        [HttpPut]
        public ProductViewModel Put(ProductViewModel product)
        {
            ProductService.SetUser(User);
            return ProductService.UpdateProduct(product);
        }

        [HttpDelete]
        public ProductViewModel Delete(int productId)
        {
            ProductService.SetUser(User);
            return ProductService.DeleteProduct(productId);
        }
    }
}
