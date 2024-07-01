using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class GetParameterGroupWithParamsRequest : IRequest<ResponseBase<ParameterGroupWithParamsResult>>
{
    public int? ParameterGroupId { get; set; }
    public string? ParameterGroupCode { get; set; }
}
