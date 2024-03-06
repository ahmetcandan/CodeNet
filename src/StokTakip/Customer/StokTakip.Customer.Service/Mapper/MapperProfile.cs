using AutoMapper;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Mapper
{
    public class MapperProfile : Profile
    {
        private readonly int depth = 3;

        public MapperProfile()
        {
            CreateMap<CreateCustomerRequest, Model.Customer>().MaxDepth(depth);
            CreateMap<UpdateCustomerRequest, Model.Customer>().MaxDepth(depth);
            CreateMap<Model.Customer, CustomerResponse>().MaxDepth(depth);
        }
    }
}
