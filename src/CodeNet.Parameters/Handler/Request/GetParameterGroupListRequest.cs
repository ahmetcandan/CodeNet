using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class GetParameterGroupListRequest : IRequest<ResponseBase<List<ParameterGroupResult>>>
{
    public int Page { get; set; }
    public int Count { get; set; }
}
