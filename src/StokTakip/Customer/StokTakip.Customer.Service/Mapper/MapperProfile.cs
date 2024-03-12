using AutoMapper;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Mapper;

public class MapperProfile : Profile
{
    private readonly int _depth = 3;

    public MapperProfile()
    {
        CreateMap<CreateCustomerRequest, Model.Customer>().MaxDepth(_depth);
        CreateMap<UpdateCustomerRequest, Model.Customer>().MaxDepth(_depth);
        CreateMap<Model.Customer, CustomerResponse>().MaxDepth(_depth);
    }
}
