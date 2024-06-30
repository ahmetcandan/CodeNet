using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class DeleteParameterRequest : IRequest<ResponseBase<ParameterResult>>
{
    public int ParameterId { get; set; }
}
