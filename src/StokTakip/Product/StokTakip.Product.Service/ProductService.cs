using NetCore.Abstraction;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.AddAsync(new Model.Product
            {
                Barcode = request.Barcode,
                CategoryId = request.CategoryId,
                Code = request.Code,
                Description = request.Description,
                Name = request.Name,
                IsActive = true,
                IsDeleted = false,
                TaxRate = request.TaxRate
            }, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return new ProductResponse
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

        public async Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync([productId], cancellationToken);
            result.IsDeleted = true;
            _productRepository.Update(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return new ProductResponse
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

        public async Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync([productId], cancellationToken);
            return new ProductResponse
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

        public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync([request.Id], cancellationToken);
            result.Barcode = request.Barcode;
            result.CategoryId = request.CategoryId;
            result.Code = request.Code;
            result.Description = request.Description;
            result.Name = request.Description;
            result.Name = request.Name;
            result.TaxRate = request.TaxRate;
            _productRepository.Update(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return new ProductResponse
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
