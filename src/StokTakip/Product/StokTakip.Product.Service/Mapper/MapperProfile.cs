using AutoMapper;
using StokTakip.Product.Contract.Request;
using StokTakip.Product.Contract.Response;

namespace StokTakip.Product.Service.Mapper
{
    public class MapperProfile : Profile
    {
        private readonly int depth = 3;

        public MapperProfile()
        {
            CreateMap<CreateProductRequest, Model.Product>().MaxDepth(depth);
            CreateMap<UpdateProductRequest, Model.Product>().MaxDepth(depth);
            CreateMap<Model.Product, ProductResponse>().MaxDepth(depth);
            CreateMap<Model.ViewModel.ProductInfo, ProductInfoResponse>().MaxDepth(depth);
        }
    }
}
