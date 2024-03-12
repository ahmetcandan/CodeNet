using AutoMapper;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Mapper;

public class MapperProfile : Profile
{
    private readonly int _depth = 3;

    public MapperProfile()
    {
        CreateMap<CreateProductRequest, Model.Product>().MaxDepth(_depth);
        CreateMap<UpdateProductRequest, Model.Product>().MaxDepth(_depth);
        CreateMap<Model.Product, ProductResponse>().MaxDepth(_depth);
        CreateMap<Model.ViewModel.ProductInfo, ProductInfoResponse>().MaxDepth(_depth);
    }
}
