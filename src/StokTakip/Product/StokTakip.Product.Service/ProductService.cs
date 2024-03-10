using NetCore.Abstraction;
using NetCore.ExceptionHandling;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;
using StokTakip.Product.Service.Mapper;

namespace StokTakip.Product.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IAutoMapperConfiguration _mapper;

        public ProductService(IProductRepository productRepository, IAutoMapperConfiguration mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var model = _mapper.MapObject<CreateProductRequest, Model.Product>(request);
            var result = await _productRepository.AddAsync(model, cancellationToken);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return _mapper.MapObject<Model.Product, ProductResponse>(result);
        }

        public async Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync([productId], cancellationToken);
            _productRepository.Remove(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return _mapper.MapObject<Model.Product, ProductResponse>(result);
        }

        public async Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetProduct(productId, cancellationToken);
            if (result is null)
                throw new UserLevelException("01", "Ürün bulunamadı!");

            return _mapper.MapObject<Model.ViewModel.ProductInfo, ProductResponse>(result);
        }

        public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            var result = await _productRepository.GetAsync([request.Id], cancellationToken);
            result.Barcode = request.Barcode;
            result.CategoryId = request.CategoryId;
            result.Code = request.Code;
            result.Description = request.Description;
            result.Name = request.Name;
            result.TaxRate = request.TaxRate;
            _productRepository.Update(result);
            await _productRepository.SaveChangesAsync(cancellationToken);
            return _mapper.MapObject<Model.Product, ProductResponse>(result);
        }
    }
}
