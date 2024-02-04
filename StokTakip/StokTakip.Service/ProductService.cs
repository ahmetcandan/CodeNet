using NetCore.Abstraction;
using StokTakip.Abstraction;
using StokTakip.Model;
using System.Threading;
using System.Threading.Tasks;

namespace StokTakip.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel product, CancellationToken cancellationToken)
        {
            var result = await _productRepository.AddAsync(new EntityFramework.Models.Product
            {
                Barcode = product.Barcode,
                CategoryId = product.CategoryId,
                Code = product.Code,
                Description = product.Description,
                Name = product.Name,
                IsActive = true,
                IsDeleted = false,
                TaxRate = product.TaxRate
            }, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);
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

        public async Task<ProductViewModel> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync(productId, cancellationToken);
            result.IsDeleted = true;
            _productRepository.Update(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
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

        public async Task<ProductViewModel> GetProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync(productId, cancellationToken);
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

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel product, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync(product.Id, cancellationToken);
            result.Barcode = product.Barcode;
            result.CategoryId = product.CategoryId;
            result.Code = product.Code;
            result.Description = product.Description;
            result.Name = product.Description;
            result.Name = product.Name;
            result.TaxRate = product.TaxRate;
            _productRepository.Update(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
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
