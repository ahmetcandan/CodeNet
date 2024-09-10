using CodeNet.ExceptionHandling;
using CodeNet.Mapper.Services;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service;

public class ProductService(IProductRepository productRepository, ICodeNetMapper mapper) : IProductService
{
    public async Task<ProductResponse> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var model = mapper.MapTo<CreateProductRequest, Model.Product>(request);
        var result = await productRepository.AddAsync(model, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapTo<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetAsync([productId], cancellationToken);
        productRepository.Remove(result);
        await productRepository.SaveChangesAsync(cancellationToken);
        return mapper.MapTo<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetProduct(productId, cancellationToken) ?? throw new UserLevelException("01", "Ürün bulunamadı!");
        return mapper.MapTo<Model.ViewModel.ProductInfo, ProductResponse>(result);
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
        return mapper.MapTo<Model.Product, ProductResponse>(result);
    }
}
