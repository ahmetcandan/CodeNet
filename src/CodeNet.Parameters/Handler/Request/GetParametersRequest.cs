using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class GetParametersRequest : IRequest<ResponseBase<List<ParameterListItemResult>>>
{
    public int? ParameterGroupId { get; set; }
    public string? ParameterGroupCode { get; set; }
}
