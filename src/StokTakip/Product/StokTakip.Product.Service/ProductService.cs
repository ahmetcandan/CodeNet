using NetCore.Abstraction;
using NetCore.ExceptionHandling;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;
using StokTakip.Product.Service.Mapper;

namespace StokTakip.Product.Service;

public class ProductService(IProductRepository productRepository, IAutoMapperConfiguration mapper) : BaseService, IProductService
{
    public async Task<ProductResponse> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var model = mapper.MapObject<CreateProductRequest, Model.Product>(request);
        var result = await productRepository.AddAsync(model, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetAsync([productId], cancellationToken);
        productRepository.Remove(result);
        await productRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetProduct(productId, cancellationToken);
        return result is null
            ? throw new UserLevelException("01", "Ürün bulunamadı!")
            : mapper.MapObject<Model.ViewModel.ProductInfo, ProductResponse>(result);
    }

    public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetAsync([request.Id], cancellationToken);
        result.Barcode = request.Barcode;
        result.CategoryId = request.CategoryId;
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.TaxRate = request.TaxRate;
        productRepository.Update(result);
        await productRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapObject<Model.Product, ProductResponse>(result);
    }
}
