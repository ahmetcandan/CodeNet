using CodeNet.Core;
using CodeNet.Core.Models;
using CodeNet.Parameters.Attributes;
using CodeNet.Parameters.Handler.Request;
using CodeNet.Parameters.Models;
using CodeNet.Parameters.Repositories;
using MediatR;

namespace CodeNet.Parameters.Handler;

internal class GetParameterGroupListHandler(ParametersDbContext dbContext, ICodeNetHttpContext identityContext) : IRequestHandler<GetParameterGroupListRequest, ResponseBase<List<ParameterGroupResult>>>
{
    private readonly ParameterGroupRepository _parameterGroupRepository = new(dbContext, identityContext);

    [ParameterCache]
    public async Task<ResponseBase<List<ParameterGroupResult>>> Handle(GetParameterGroupListRequest request, CancellationToken cancellationToken)
    {
        var list = (await _parameterGroupRepository.GetPagingListAsync(request.Page, request.Count, cancellationToken)).Select(c => new ParameterGroupResult
        {
            Code = c.Code,
            ApprovalRequired = c.ApprovalRequired,
            Description = c.Description,
            Id = c.Id
        }).ToList();

        return new ResponseBase<List<ParameterGroupResult>>(list);
    }
}
