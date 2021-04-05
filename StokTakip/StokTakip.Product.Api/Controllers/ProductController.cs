﻿using Microsoft.AspNetCore.Mvc;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;

namespace StokTakip.Product.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        IProductService ProductService;

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet]
        public List<ProductViewModel> GetAll()
        {
            ProductService.SetUser(User);
            return ProductService.GetProducts();
        }

        [HttpGet]
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
