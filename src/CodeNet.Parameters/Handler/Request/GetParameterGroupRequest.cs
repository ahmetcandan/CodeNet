using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class GetParameterGroupRequest : IRequest<ResponseBase<ParameterGroupResult>>
{
    public int? ParameterGroupId { get; set; }
    public string? ParameterGroupCode { get; set; }
}
