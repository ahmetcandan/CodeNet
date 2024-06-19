using MediatR;
using CodeNet.Core.Models;
using StokTakip.Customer.Abstraction.Service;
using StokTakip.Customer.Contract.Request;
using StokTakip.Customer.Contract.Response;
using Microsoft.Extensions.Logging;
using CodeNet.Logging;
using System.Reflection;

namespace StokTakip.Customer.Service.Handler;

public class GetCustomerHandler(ICustomerService CustomerService, ILogger<GetCustomerHandler> Logger, IAppLogger AppLogger) : IRequestHandler<GetCustomerRequest, ResponseBase<CustomerResponse>>
{
    public async Task<ResponseBase<CustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Logger mesajı");
        AppLogger.TraceLog("AppLogger mesajı", MethodBase.GetCurrentMethod());
        return new ResponseBase<CustomerResponse>(await CustomerService.GetCustomer(request.Id, cancellationToken));
    }
}
