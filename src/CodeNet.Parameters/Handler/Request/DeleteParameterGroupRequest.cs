using CodeNet.Core.Models;
using CodeNet.Parameters.Models;
using MediatR;

namespace CodeNet.Parameters.Handler.Request;

internal class DeleteParameterGroupRequest : IRequest<ResponseBase<ParameterGroupResult>>
{
    public int ParameterGroupId { get; set; }
}
