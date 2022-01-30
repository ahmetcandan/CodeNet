using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Collections.Generic;
using System.Linq;

namespace StokTakip.Service
{
    public class ProductService : BaseService, IProductService
    {
        IProductRepository productRepository;
        ILogRepository logRepository;
        IQService qService;

        public ProductService(IProductRepository productRepository, ILogRepository logRepository, IQService qService)
        {
            this.productRepository = productRepository;
            this.qService = qService;
            this.logRepository = logRepository;
        }

        public ProductViewModel CreateProduct(ProductViewModel product)
        {
            var result = productRepository.Add(new EntityFramework.Models.Product
            {
                Barcode = product.Barcode,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Description = product.Description,
                Name = product.Name,
                IsActive = true,
                IsDeleted = false,
                TaxRate = product.TaxRate
            });
            return new ProductViewModel
            {
                Barcode = result.Barcode,
                CategoryId = result.CategoryId,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                TaxRate = result.TaxRate,
                Id = result.Id
            };
        }

        public ProductViewModel DeleteProduct(int productId)
        {
            var result = productRepository.Get(productId);
            result.IsDeleted = true;
            productRepository.Update(result);
            return new ProductViewModel
            {
                Barcode = result.Barcode,
                CategoryId = result.CategoryId,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                TaxRate = result.TaxRate,
                Id = result.Id
            };
        }

        public ProductViewModel GetProduct(int productId)
        {
            var result = productRepository.Get(productId);
            return new ProductViewModel
            {
                Barcode = result.Barcode,
                CategoryId = result.CategoryId,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                TaxRate = result.TaxRate,
                Id = result.Id
            };
        }

        public List<ProductViewModel> GetProducts()
        {
            return (
                    from c in productRepository.GetAll()
                    select new ProductViewModel
                    {
                        Barcode = c.Barcode,
                        CategoryId = c.CategoryId,
                        Code = c.Code,
                        Description = c.Description,
                        Id = c.Id,
                        Name = c.Name,
                        TaxRate = c.TaxRate
                    }
                ).ToList();
        }

        public ProductViewModel UpdateProduct(ProductViewModel product)
        {
            var result = productRepository.Get(product.Id);
            result.Barcode = product.Barcode;
            result.CategoryId = product.CategoryId;
            result.Code = product.Code;
            result.Description = product.Description;
            result.Name = product.Description;
            result.Name = product.Name;
            result.TaxRate = product.TaxRate;
            productRepository.Update(result);
            return new ProductViewModel
            {
                Barcode = result.Barcode,
                CategoryId = result.CategoryId,
                Code = result.Code,
                Description = result.Description,
                Name = result.Name,
                TaxRate = result.TaxRate,
                Id = result.Id
            };
        }
    }
}
