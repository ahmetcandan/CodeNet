﻿using MediatR;
using NetCore.Abstraction.Model;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;

namespace StokTakip.Customer.Service.Handler;

public class GetCustomerHandler(ICustomerService customerService) : IRequestHandler<GetCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await customerService.GetCustomer(request.Id, cancellationToken);
        return new ResponseBase<CustomerResponse>
        {
            Data = customer,
            IsSuccessfull = true
        };
    }
}
