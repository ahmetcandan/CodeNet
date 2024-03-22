using NetCore.Abstraction;
using NetCore.ExceptionHandling;
using StokTakip.Product.Abstraction.Repository;
using StokTakip.Product.Abstraction.Service;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;
using StokTakip.Product.Service.Mapper;

namespace StokTakip.Product.Service;

public class ProductService(IProductRepository ProductRepository, IAutoMapperConfiguration Mapper) : BaseService, IProductService
{
    public async Task<ProductResponse> CreateProduct(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var model = Mapper.MapObject<CreateProductRequest, Model.Product>(request);
        var result = await ProductRepository.AddAsync(model, cancellationToken);
        await ProductRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> DeleteProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await ProductRepository.GetAsync([productId], cancellationToken);
        ProductRepository.Remove(result);
        await ProductRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Product, ProductResponse>(result);
    }

    public async Task<ProductResponse> GetProduct(int productId, CancellationToken cancellationToken)
    {
        var result = await ProductRepository.GetProduct(productId, cancellationToken) ?? throw new UserLevelException("01", "Ürün bulunamadı!");
        return Mapper.MapObject<Model.ViewModel.ProductInfo, ProductResponse>(result);
    }

    public async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var result = await ProductRepository.GetAsync([request.Id], cancellationToken);
        result.Barcode = request.Barcode;
        result.CategoryId = request.CategoryId;
        result.Code = request.Code;
        result.Description = request.Description;
        result.Name = request.Name;
        result.TaxRate = request.TaxRate;
        ProductRepository.Update(result);
        await ProductRepository.SaveChangesAsync(cancellationToken);
        return Mapper.MapObject<Model.Product, ProductResponse>(result);
    }
}
